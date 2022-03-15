using Microsoft.EntityFrameworkCore;
using ServiceManager.Core.Interfaces;
using ServiceManager.Core.Services;
using ServiceManager.Data;
using ServiceManager.Data.Entities;
using System.Configuration;
using AutoMapper;
using Hangfire;
using ServiceManager.Infrastructure;
using ServiceManager.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connection = builder.Configuration.GetConnectionString("Cnn");
//Hang fire below
//var connectString = builder.Configuration.GetConnectionString("Cnn");
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connection));
//builder.Services.AddHangfireServer();
builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[] { "alpha", "beta", "default" };
});





builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IGenRepo<Service,int>, ServiceRepo>();
builder.Services.AddScoped<IEmailSender, EmailSenderServices>();
//
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
//builder.Services.AddAutoMapper();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
//use this for gb
app.UseHangfireDashboard();
//Recurrent Job
RecurringJob.AddOrUpdate<IGenRepo<Service,int>>(x => x.MonitorServiceAlert(), Cron.MinuteInterval(5));
//Fire and Forget
//BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget Job Executed"));  
//Dlayed
//BackgroundJob.Schedule(() => Console.WriteLine("Delayed job executed"), TimeSpan.FromMinutes(1));  



app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Service}/{action=ServiceMgt}/{id?}");
//pattern: "{controller=Home}/{action=Index}/{id?}");
//ServiceMgt
app.Run();
////using System.Linq;
////using System.Threading.Tasks;

////namespace ServiceManager.Web.ViewModels
////{
////    public class Program
////    {
////        public static void Main(string[] args)
////        {
////            CreateHostBuilder(args).Build().Run();
////        }

////        public static IHostBuilder CreateHostBuilder(string[] args) =>
////            Host.CreateDefaultBuilder(args)
////                .ConfigureWebHostDefaults(webBuilder =>
////                {
////                    webBuilder.UseStartup<Startup>();
////                });
////    }
////}

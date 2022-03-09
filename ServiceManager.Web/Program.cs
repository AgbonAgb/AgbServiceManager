using Microsoft.EntityFrameworkCore;
using ServiceManager.Core.Interfaces;
using ServiceManager.Core.Services;
using ServiceManager.Data;
using ServiceManager.Data.Entities;
using System.Configuration;
using AutoMapper;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connection = builder.Configuration.GetConnectionString("Cnn");
//Hang fire below
//var connectString = builder.Configuration.GetConnectionString("Cnn");
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connection));
builder.Services.AddHangfireServer();



builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IGenRepo<Service,int>, ServiceRepo>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
RecurringJob.AddOrUpdate<IGenRepo<Service,int>>(x => x.MonitorServiceAlert(), Cron.MinuteInterval(30));


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

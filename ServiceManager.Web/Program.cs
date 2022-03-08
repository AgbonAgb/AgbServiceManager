using Microsoft.EntityFrameworkCore;
using ServiceManager.Core.Interfaces;
using ServiceManager.Core.Services;
using ServiceManager.Data;
using ServiceManager.Data.Entities;
using System.Configuration;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connection = builder.Configuration.GetConnectionString("Cnn");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IGenRepo<Service,int>, ServiceRepo>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddAutoMapper();

var app = builder.Build();





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

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

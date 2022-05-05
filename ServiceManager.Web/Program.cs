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
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration.UserSecrets;
using NLog.Web;
using NLog;
using Microsoft.Extensions.Logging;
//using EFCoreMySQL.DBContexts;


string filepath = "C:\\Logs";
if (!System.IO.File.Exists(filepath))
{
    Directory.CreateDirectory(filepath);
}
//get this file
var path = string.Concat(Directory.GetCurrentDirectory(), "\\NLog.config");
var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();

//var logger = NLog.LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();
//"C:\\Users\\Godwin\\source\repos\\AgbServiceManager\\ServiceManager.Web\nlog.config.txt"
logger.Debug("init main");


//LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

//var path = Directory.GetCurrentDirectory();
//LoggerFactory.AddFile($"{path}\\Logs\\Log.txt");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();
    //
    //builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());


    //IConfiguration configuration = new ConfigurationBuilder()
    //                            .SetBasePath(Directory.GetCurrentDirectory())
    //                            .AddJsonFile("appsettings.json")
    //                            .AddUserSecrets("secretes.json")
    //                            .AddEnvironmentVariables()
    //                            .Build();

    //Private IConfiguration _config;

    //var ho = configuration.GetConnectionString("Data source=10.12.201.66;user id=sa; Password=Test_test1;Database=ServiceMonitor");
   // string conn = "Server=10.12.201.66;user id=sa; Password=Test_test1;Database=ServiceMonitor";
    var connection2 = builder.Configuration.GetConnectionString("DefaultConnection");
    //var connection2 = builder.Configuration["Servicemanager:ConnectionString"];
    //var EmailPassword = builder.Configuration["Servicemanager:EmailPassword"];
    //Hang fire below

    builder.Services.AddHangfire(x => x.UseSqlServerStorage(connection2));
    //builder.Services.AddHangfireServer();
    builder.Services.AddHangfireServer(options =>
    {
        options.Queues = new[] { "alpha", "beta", "default" };
    });

    //


    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection2));
    //mySql
   // builder.Services.AddDbContextPool<AppDbContext>(options => options.UseMySql(connection2, ServerVersion.AutoDetect(connection2)));
    //.ServerVersion(new Version(8, 0, 19), ServerType.MySql))

    builder.Services.AddScoped<IGenRepo<Service, int>, ServiceRepo>();
    builder.Services.AddScoped<IEmailSender, EmailSenderServices>();
    //
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
    builder.Services.Configure<DefaultEmail>(builder.Configuration.GetSection("AdminEmail"));
    //builder.Services.Configure<EmailPassword>(EmailPassword);
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

    //Migration
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // use context
        dbContext.Database.EnsureCreated();
        //dbContext.Database.Migrate();
    }

    //use this for gb
    app.UseHangfireDashboard();
    //Recurrent Job
   RecurringJob.AddOrUpdate<IGenRepo<Service, int>>(x => x.MonitorServiceAlert(), Cron.Daily());
    //RecurringJob.AddOrUpdate<IGenRepo<Service, int>>(x => x.MonitorServiceAlert(), Cron.HourInterval(24));
    //RecurringJob.AddOrUpdate<IGenRepo<Service, int>>(x => x.MonitorServiceAlert(), Cron.MinuteInterval(4));


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
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}


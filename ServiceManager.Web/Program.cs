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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
var connection = builder.Configuration.GetConnectionString("Cnn");

var connection2 = builder.Configuration["Servicemanager:ConnectionString"];
var EmailPassword = builder.Configuration["Servicemanager:EmailPassword"];
//Hang fire below
//var connectString = builder.Configuration.GetConnectionString("Cnn");
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connection2));
//builder.Services.AddHangfireServer();
builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[] { "alpha", "beta", "default" };
});




builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection2));
builder.Services.AddScoped<IGenRepo<Service,int>, ServiceRepo>();
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
    dbContext.Database.Migrate();
}

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


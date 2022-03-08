using Microsoft.EntityFrameworkCore;
using ServiceManager.Data;

namespace ServiceManager.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //To authorize all controllers
            services.AddControllersWithViews();// (o => o.Filters.Add(new AuthorizeFilter()));
            //services.Configure<FormOptions>(x => x.ValueCountLimit = 100000000);

            //services.AddControllersWithViews();//Authorize controllers o =>o.Filters.Add(new AuthorizeFilter())
            //services.AddRazorPages().AddMvcOptions(o => o.Filters.Add(new AuthorizeFilter()));//authorize all pages


            //services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            //{

            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequiredLength = 8;

            //})
            //.AddSignInManager<SignInManager<ApplicationUser>>()
            //.AddUserManager<UserManager<ApplicationUser>>()
            //.AddRoleManager<RoleManager<ApplicationRole>>()
            //.AddEntityFrameworkStores<AppDbContext>();



            //services.AddScoped<IBiodata, BiodataServices>();
            //services.AddScoped<IUserRepository, UserRepositoryServices>();
            //services.AddScoped<IDept, DeptServices>();
            //services.AddScoped<IPurchaseOrder, PurchaseOrderServices>();
            ////this handles logins and related operations
            //services.AddScoped<IAccounts, AccountServices>();
            //services.AddScoped<ICategory, CategoryServices>();
            //services.AddScoped<ISkills, SkillServices>();
            //services.AddScoped<ICareer, CareerServices>();
            //services.AddScoped<IApplication, ApplicationServices>();
            //services.AddScoped<IYearsExperienceCost, YearsExperienceCostService>();
            //services.AddScoped<IShoppingCartItem, ShoppingCartItemServices>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            //services.AddSingleton<IEmailSender, EmailSenderServices>();
            //services.Add<HttpContextAccessor>();


            //services.AddAutoMapper();

            var connection = Configuration.GetConnectionString("Cnn");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            //using Coogies//;//we could add this o => o.LoginPath="account/signin. Stop at .AddCookie(); if no external authentication required
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            ////services.Configure<CookiePolicyOptions>(options =>
            ////{
            ////    // This lambda determines whether user consent for non-essential cookies is needed for a given request.  
            ////    options.CheckConsentNeeded = context => true;
            ////    options.MinimumSameSitePolicy = SameSiteMode.None;

            ////    // Cookie settings
            ////    //options.HttpOnly = true;
            ////    ////options.Cookie.Expiration 

            ////    //options.Secure..ExpireTimeSpan = TimeSpan.FromMinutes(5);
            ////    //options.LoginPath = "/Identity/Account/Login";
            ////    //options.LogoutPath = "/Identity/Account/Logout";
            ////    //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            ////    //options.SlidingExpiration = true;
            ////});
            ////services.AddAuthentication(o =>
            ////{
            ////    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            ////    //this will challenge user to google login page if not authenticated and want to bypass
            ////    // o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            ////})
            ////    .AddCookie()
            ////    .AddCookie(ExternalAuthenticationDefaults.AuthenticationScheme)
            ////    .AddGoogle(o =>
            ////    {
            ////        o.SignInScheme = ExternalAuthenticationDefaults.AuthenticationScheme;
            ////        o.ClientId = Configuration["Google:ClientId"];
            ////        o.ClientSecret = Configuration["Google:ClientSecret"];
            ////    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                //context.Database.Migrate();
                context.Database.EnsureCreated(); //.Migrate();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            if (env.IsProduction())
            {
                // context.Database.Migrate();
                context.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();//added for authentication
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
                //pattern: "{controller=Account}/{action=Login}/{id?}");
            });
            //Seed data
           // CreateUserRoles(serviceProvider).Wait();
            //SampleData.Initialize(app, serviceProvider);
            // PrepDb.prepopulate(app);
        }


        //private async Task CreateUserRoles(IServiceProvider serviceProvider)
        //{
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        //    //var user = new User() { Email = "admin@gmail.com", UserName = "admin" };

        //    var user = new ApplicationUser
        //    {
        //        FirstName = "Godwin",
        //        LastName = "Agbon",
        //        Email = "agbonwinn1@yahoo.com",
        //        NormalizedEmail = "AGBONWINN1@YAHOO.COM",
        //        UserName = "Godwin",
        //        NormalizedUserName = "GODWIN",
        //        PhoneNumber = "",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true,
        //        SecurityStamp = Guid.NewGuid().ToString("D")
        //    };
        //    var result = await UserManager.CreateAsync(user, "Pa$$word123");

        //    var roleCheck = await RoleManager.RoleExistsAsync("Admin");
        //    var role = new ApplicationRole();
        //    role.Name = "Admin";
        //    if (!roleCheck)
        //    {
        //        //create the roles and seed them to the database  
        //        await RoleManager.CreateAsync(role);
        //    }
        //    await UserManager.AddToRoleAsync(user, "Admin");

        //}

    }
}

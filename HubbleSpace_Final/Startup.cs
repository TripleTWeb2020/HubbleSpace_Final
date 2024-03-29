using AspNetCore.SEOHelper;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Helpers;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Repository;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HubbleSpace_Final
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
            services.AddControllersWithViews();
            services.AddDbContext<MyDbContext>(option => { option.UseSqlServer(Configuration.GetConnectionString("HubbleSpace_Final")); });
            services.AddIdentity<ApplicationUser, IdentityRole>()

                .AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();
            services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
                        options.ClientId = "97229891671-sqgug92g9ld9g3d6hg7njtc1pvs852m2.apps.googleusercontent.com";
                        options.ClientSecret = "u_ENyz7ETZKJPlGcSPWtYYlA";
                        options.SignInScheme = IdentityConstants.ExternalScheme;
                    })
                    .AddFacebook(options =>
                    {
                        IConfigurationSection facebookAuthNSection = Configuration.GetSection("Authentication:Facebook");
                        options.AppId = "385123116535823";
                        options.AppSecret = "341b7385c231b8e878cb389d61d2a39b";
                        options.SignInScheme = IdentityConstants.ExternalScheme;
                    });
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = Configuration["Application:LoginPath"];
            });
            services.AddDistributedMemoryCache();
            services.AddSession(cfg =>
            {
                cfg.Cookie.Name = "hubblespaceteam";
                cfg.IdleTimeout = new TimeSpan(0, 30, 0);

            });
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 5;

            });
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IEmailService, EmailService>();
            //services.AddIdentity<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddHttpClient();
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseSession();

            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Admin",
                    pattern: "Admin/Index", new { controller = "Admin", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "Product_Detail",
                    pattern: "{name}/{color}", new { controller = "Home", action = "Product_Detail" });

                 endpoints.MapControllerRoute(
                    name: "Categories",
                    pattern: "{Object}-{name}", new { controller = "Home", action = "Categories" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseXMLSitemap(env.ContentRootPath);
        }
    }
}

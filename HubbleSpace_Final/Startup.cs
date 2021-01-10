using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Helpers;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Repository;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                        options.ClientId = Configuration["App:GoogleClientId"];
                        options.ClientSecret = Configuration["App:GoogleClientSecret"];
                    })
                    .AddFacebook(options => {
                        options.ClientId = Configuration["App:FacebookClientId"];
                        options.ClientSecret = Configuration["App:FacebookClientSecret"];
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
            });
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IEmailService, EmailService>();
            //services.AddIdentity<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddHttpClient();
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

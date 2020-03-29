using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using NETCore.MailKit.Core;
using Microsoft.Extensions.Configuration;
using Identity_EmailVarification.Data;
using Identity_EmailVarification.Services;

namespace Identity_EmailVarification
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration config)
        {
            configuration = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var gmailOptions = configuration.GetSection("Gmail").Get<Gmail>();

            services.AddGmailService(opts=>
            {
                opts.SourceEmail = gmailOptions.SourceEmail;
                opts.Password = gmailOptions.Password;
            });

            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseInMemoryDatabase("InMemoryDb");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.cookie";
                config.LoginPath = "/Home/Login";
            });

            var mailKitOptions = configuration.GetSection("Email").Get<MailKitOptions>();

            services.AddMailKit(config =>
            {
                config.UseMailKit(mailKitOptions);
            });
            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

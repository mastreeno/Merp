// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using Merp.Web.Auth.Controllers;
using Merp.Web.Auth.Data;
using Merp.Web.Auth.Models;
using Merp.Web.Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Reflection;

namespace Merp.Web.Auth
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AspNetIdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();

            var config = new Config(Configuration);
            services.AddSingleton(config);

            var bootstrapper = new AppBootstrapper(Configuration, services);
            bootstrapper.Configure();

            services.AddAuthorization();
            services
                .AddAuthentication()
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = Configuration["IdentityServerEndpoints:Authority"];
                    options.RequireHttpsMetadata = true;

                    options.ApiName = "merp.auth.api";
                });
                //.AddGoogle(options =>
                //{
                //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                //    // register your IdentityServer with Google at https://console.developers.google.com
                //    // enable the Google+ API
                //    // set the redirect URI to https://localhost:5001/signin-google
                //    options.ClientId = "copy client ID from Google here";
                //    options.ClientSecret = "copy client secret from Google here";
                //});
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();
            app.UseCors(builder =>
                builder
                    //.WithOrigins(Configuration["ClientEndpoints:WebApp"], Configuration["ClientEndpoints:WasmApp"])
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "i18n",
                    pattern: "api/private/i18n/{controllerResourceName}/{actionResourceName}",
                    defaults: new { controller = "Config", action = nameof(ConfigController.GetLocalization) });

                endpoints.MapControllerRoute(
                    name: "managePrivateApi",
                    pattern: "api/private/Manage/{action=Index}/{id?}",
                    defaults: new { controller = "Manage" });

                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
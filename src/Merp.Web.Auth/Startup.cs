using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Merp.Web.Auth.Configuration;
using Merp.Web.Auth.Data;
using Merp.Web.Site.Models;
using Merp.Web.Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Merp.Web.Auth
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("AspNetIdentityConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            RegisterClientsCors(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpContextAccessor();

            services.AddTransient<IEmailSender, EmailSender>();

            var _clientsConfiguration = new Dictionary<string, ClientConfiguration>();
            var _apiResourcesConfiguration = new List<ApiResourceConfiguration>();
            Configuration.GetSection("ClientConfigurations").Bind(_clientsConfiguration);
            Configuration.GetSection("ApiResourceConfigurations").Bind(_apiResourcesConfiguration);

            services.AddSingleton(new IdentityServerConfiguration(_clientsConfiguration, _apiResourcesConfiguration));
            
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseIdentityServer();

            UseClientsCors(app);
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "i18n",
                    template: "api/private/i18n/{controllerResourceName}/{actionResourceName}",
                    defaults: new { controller = "Config", action = "GetLocalization" });

                routes.MapRoute(
                    name: "managePrivateApi",
                    template: "api/private/Manage/{action=Index}/{id?}",
                    defaults: new { controller = "Manage" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RegisterClientsCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("WebApp", policy =>
                {
                    policy.WithOrigins(Configuration["ClientEndpoints:WebApp"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        private void UseClientsCors(IApplicationBuilder app)
        {
            app.UseCors("WebApp");
        }
    }
}

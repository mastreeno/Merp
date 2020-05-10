using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Merp.Web;
using Merp.Accountancy.Web.Api.Public.WorkerServices;

namespace Merp.Accountancy.Web.Api.Public
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["IdentityServerEndpoints:Authority"];
                    options.RequireHttpsMetadata = true;

                    options.ApiName = "merp.accountancy.api.public";
                });


            RegisterClientsCors(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpContextAccessor();

            services.AddSingleton(services);

            var bootstrapper = new AppBootstrapper(Configuration, services);
            bootstrapper.Configure();

            services.AddScoped<InvoiceControllerWorkerServices>();
            services.AddScoped<JobOrderControllerWorkerServices>();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            UseClientsCors(app);
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "i18n",
                //    template: "api/i18n/{controllerResourceName}/{actionResourceName}",
                //    defaults: new { controller = "Config", action = "GetLocalization" });

                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}");
            });
        }

        private void RegisterClientsCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AccountancyPublicApi", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

        private void UseClientsCors(IApplicationBuilder app)
        {
            app.UseCors("AccountancyPublicApi");
        }
    }
}

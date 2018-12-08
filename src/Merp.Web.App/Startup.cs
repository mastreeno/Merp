using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Merp.Web.App.Configuration;
using Merp.Web.App.Services.Url;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Merp.Web.App
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
            services.AddScoped((serviceProvider) => 
            {
                var endpointConfiguration = new EndpointConfiguration();
                Configuration.GetSection("EndpointConfiguration").Bind(endpointConfiguration);

                return new UrlBuilder(endpointConfiguration);
            });

            services.AddAuthorization();
            services.AddAuthentication()
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = Configuration["EndpointConfiguration:Authority"];
                    options.RequireHttpsMetadata = true;

                    options.ApiName = "merp.webapp.api";
                });
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "i18n",
                    template: "api/private/i18n/{controllerResourceName}/{actionResourceName}",
                    defaults: new { controller = "Config", action = "GetLocalization" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}

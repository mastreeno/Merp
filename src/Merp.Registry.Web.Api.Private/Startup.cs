using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Merp.Web;
using Merp.Registry.Web.WorkerServices;
using Merp.Web.Cors;

namespace Merp.Registry.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddAuthorization();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["IdentityServerEndpoints:Authority"];
                    options.RequireHttpsMetadata = true;

                    options.ApiName = "merp.registry.api";
                });

            services.RegisterClientsCors(Configuration);
            
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddHttpContextAccessor();

            services.AddSingleton(services);


            services.AddScoped<Merp.Registry.Web.Core.Configuration.IBoundedContextConfigurationProvider, Merp.Registry.Web.Core.Configuration.BoundedContextConfigurationProvider>();
            services.AddSingleton<Merp.Registry.Web.AppBootstrapper>();
            var bootstrapper = services.BuildServiceProvider().GetService<Merp.Registry.Web.AppBootstrapper>();
            bootstrapper.Configure();

            services.AddScoped<PartyControllerWorkerServices>();
            services.AddScoped<PersonControllerWorkerServices>();
            services.AddScoped<CompanyControllerWorkerServices>();
        }

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
            app.UseClientsCors();
            app.UseAuthentication();
            app.UseMvc(routes => 
            {
                routes.MapRoute(
                    name: "i18n",
                    template: "api/i18n/{controllerResourceName}/{actionResourceName}",
                    defaults: new { controller = "Config", action = "GetLocalization" });

                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}");
            });
        }
    }
}

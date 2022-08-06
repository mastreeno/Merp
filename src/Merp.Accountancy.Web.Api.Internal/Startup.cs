using Merp.Accountancy.Web.Api.Internal.WorkerServices;
using Merp.Web;
using Merp.Web.Cors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Merp.Accountancy.Web.Api.Internal
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

                    options.ApiName = "merp.accountancy.api.internal";
                });


            services.RegisterClientsCors(Configuration);

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddHttpContextAccessor();

            services.AddSingleton(services);


            services.AddScoped<Merp.Accountancy.Web.Core.Configuration.IBoundedContextConfigurationProvider, Merp.Accountancy.Web.Core.Configuration.BoundedContextConfigurationProvider>();
            services.AddSingleton<Merp.Accountancy.Web.AppBootstrapper>();
            var bootstrapper = services.BuildServiceProvider().GetService<Merp.Accountancy.Web.AppBootstrapper>();
            bootstrapper.Configure();

            services.AddScoped<VatControllerWorkerServices>();
            services.AddScoped<ProvidenceFundControllerWorkerServices>();
            services.AddScoped<WithholdingTaxControllerWorkerServices>();
            services.AddScoped<SettingsControllerWorkerServices>();
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

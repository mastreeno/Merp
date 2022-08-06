using Merp.Accountancy.Web.Api.Public.WorkerServices;
using Merp.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen.ConventionalRouting;

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

            services.AddMvc(option => option.EnableEndpointRouting = true);
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Merp public API - Accountancy", Version = "v1" });
            });
            services.AddSwaggerGenWithConventionalRoutes();

            services.AddHttpContextAccessor();

            services.AddSingleton(services);

            services.AddScoped<Merp.Accountancy.Web.Core.Configuration.IBoundedContextConfigurationProvider, Merp.Accountancy.Web.Core.Configuration.BoundedContextConfigurationProvider>();
            services.AddSingleton<Merp.Accountancy.Web.AppBootstrapper>();
            var bootstrapper = services.BuildServiceProvider().GetService<Merp.Accountancy.Web.AppBootstrapper>();
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Merp public API V1 - Accountancy");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action}/{id?}");

                ConventionalRoutingSwaggerGen.UseRoutes(endpoints);
            });
            //app.UseMvc(routes =>
            //{
            //    //routes.MapRoute(
            //    //    name: "i18n",
            //    //    template: "api/i18n/{controllerResourceName}/{actionResourceName}",
            //    //    defaults: new { controller = "Config", action = "GetLocalization" });

            //    routes.MapRoute(
            //        name: "default",
            //        template: "api/{controller}/{action}/{id?}");

            //    ConventionalRoutingSwaggerGen.UseRoutes(routes.Routes.ToList());
            //});
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Merp.Martin.Web.Alexa
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
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<Merp.Accountancy.QueryStack.AccountancyDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Merp-Accountancy-ReadModel")));
            services.AddDbContext<Merp.Accountancy.Drafts.AccountancyDraftsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Merp-Accountancy-Drafts")));
            services.AddDbContext<Merp.Registry.QueryStack.RegistryDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Merp-Registry-ReadModel")));
            services.AddDbContext<Merp.TimeTracking.TaskManagement.QueryStack.TaskManagementDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Merp-TimeTracking-ReadModel")));

            services.AddScoped<Merp.Martin.Intents.Accountancy.GrossIncomeIntentWorker>();
            services.AddScoped<Merp.Martin.Intents.Accountancy.NetIncomeIntentWorker>();
            services.AddScoped<Merp.Martin.Intents.Accountancy.OutgoingInvoicePaymentCheckWorker>();

            services.AddScoped<Merp.Martin.Intents.General.HelpWorker>();
            services.AddScoped<Merp.Martin.Intents.General.GreetingWorker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();

            //app.UseMvc();
            app.UseMiddleware<AlexaMiddleware>();
        }
    }
}

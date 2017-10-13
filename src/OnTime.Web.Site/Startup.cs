using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using OnTime.Web.Site.Data;
using OnTime.Web.Site.Models;
using OnTime.Web.Site.Services;
using OnTime.Web.Site.Areas.App.WorkerServices;

namespace OnTime.Web.Site
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            //Add WorkerServices
            services.AddScoped<TaskControllerWorkerServices>();

            //Memento wire-up
            //services.AddSingleton(services);

            var bootstrapper = new AppBootstrapper(Configuration, services);
            bootstrapper.ConfigureBus();
            bootstrapper.ConfigureEventStore();
            bootstrapper.ConfigureReadModel();
            bootstrapper.RegisterSagas();          
            bootstrapper.RegisterDenormalizers();
            bootstrapper.RegisterHandlers();
            

            //Worker role subscriptions
            var container = services.BuildServiceProvider();
            var bus = container.GetService<IBus>();
            bus.Subscribe<OnTime.TaskManagement.CommandStack.Events.TaskCreatedEvent>();
            bus.Subscribe<OnTime.TaskManagement.CommandStack.Events.TaskCompletedEvent>();
            bus.Subscribe<OnTime.TaskManagement.CommandStack.Events.TaskCancelledEvent>();
            bus.Subscribe<OnTime.TaskManagement.CommandStack.Events.TaskRenamedEvent>();
            bus.Subscribe<OnTime.TaskManagement.CommandStack.Events.DueDateSetForTaskEvent>();
            bus.Subscribe<OnTime.TaskManagement.CommandStack.Events.DueDateRemovedFromTaskEvent>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            var facebookOptions = new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"]
            };
            facebookOptions.Fields.Add("email");
            facebookOptions.Fields.Add("first_name");
            facebookOptions.Fields.Add("last_name");
            app.UseFacebookAuthentication(facebookOptions);

            var googleOptions = new GoogleOptions()
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"],
                Scope = { "email", "openid" }
            };
            app.UseGoogleAuthentication(googleOptions);

            var microsoftAccountOptions = new MicrosoftAccountOptions()
            {
                ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"],
                ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"]
            };
            app.UseMicrosoftAccountAuthentication(microsoftAccountOptions);

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

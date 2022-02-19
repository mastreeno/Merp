using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Merp.Web.Auth.Models;
using Merp.Web.Auth.WorkerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace Merp.Web.Auth
{
    public class AppBootstrapper
    {
        public IServiceCollection Services { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IWebHostEnvironment Environment { get; set; }

        public AppBootstrapper(IConfiguration configuration, IServiceCollection services)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = Services.BuildServiceProvider().GetService<IWebHostEnvironment>();
        }

        public void Configure()
        {
            ConfigureIdentityServer();
            SetupIdentityServerConfigurationData();
            RegisterWorkerServices();
        }

        private void RegisterWorkerServices()
        {
            Services.AddScoped<ManageControllerWorkerServices>();
        }

        private void ConfigureIdentityServer()
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var connectionString = Configuration.GetConnectionString("IdentityServerConfigurationConnection");
            var builder = Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            })
            .AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }

            #region legacy
            //var identityServerBuilder = Services
            //    .AddIdentityServer(options =>
            //    {
            //        options.Events.RaiseErrorEvents = true;
            //        options.Events.RaiseInformationEvents = true;
            //        options.Events.RaiseFailureEvents = true;
            //        options.Events.RaiseSuccessEvents = true;
            //        options.UserInteraction.LoginUrl = "/Account/Login";
            //        options.UserInteraction.LogoutUrl = "/Account/Logout";
            //        // options.IssuerUri = "https://mastreenoauth.azurewebsites.net";
            //        // options.PublicOrigin = "https://mastreenoauth.azurewebsites.net";
            //    })
            //    .AddConfigurationStore(options =>
            //    {
            //        options.ConfigureDbContext = builder => builder.UseSqlServer(
            //            Configuration.GetConnectionString("IdentityServerConfigurationConnection"),
            //            sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
            //    })
            //    .AddOperationalStore(options =>
            //    {
            //        options.ConfigureDbContext = builder => builder.UseSqlServer(
            //            Configuration.GetConnectionString("IdentityServerConfigurationConnection"),
            //            sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
            //    })
            //    .AddAspNetIdentity<ApplicationUser>();

            //if (Environment.IsDevelopment())
            //{
            //    identityServerBuilder.AddDeveloperSigningCredential();
            //}
            #endregion
        }

        private void SetupIdentityServerConfigurationData()
        {
            using var serviceScope = Services.BuildServiceProvider().GetService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var config = serviceScope.ServiceProvider.GetRequiredService<Config>();
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();

            if (!context.Clients.Any())
            {
                foreach (var client in config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in config.ApiResouces)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}

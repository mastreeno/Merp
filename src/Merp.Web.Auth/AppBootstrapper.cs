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
            //        // options.IssuerUri = "";
            //        // options.PublicOrigin = "";
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

        private void SetupIdentityServerConfigurationDataLegacy()
        {
            var identityServerConfiguration = Services.BuildServiceProvider().GetService<Config>();

            using var configurationContext = Services.BuildServiceProvider().GetService<ConfigurationDbContext>();
            configurationContext.Database.Migrate();
            foreach (var apiScope in identityServerConfiguration.ApiScopes)
            {
                if (!configurationContext.ApiScopes.Any(r => r.Name == apiScope.Name))
                {
                    configurationContext.ApiScopes.Add(apiScope.ToEntity());
                }
            }
            configurationContext.SaveChanges();

            foreach (var apiResource in identityServerConfiguration.ApiResouces)
            {
                if (!configurationContext.ApiResources.Any(r => r.Name == apiResource.Name))
                {
                    configurationContext.ApiResources.Add(apiResource.ToEntity());
                }
            }
            configurationContext.SaveChanges();

            foreach (var identityResource in identityServerConfiguration.IdentityResources)
            {
                if (!configurationContext.IdentityResources.Any(i => i.Name == identityResource.Name))
                {
                    configurationContext.IdentityResources.Add(identityResource.ToEntity());
                }
            }
            configurationContext.SaveChanges();

            var clients = identityServerConfiguration.Clients;
            foreach (var client in clients)
            {
                if (!configurationContext.Clients.Any(c => c.ClientId == client.ClientId))
                {
                    configurationContext.Clients.Add(client.ToEntity());
                }
                else
                {
                    var clientEntity = configurationContext.Clients
                        .Include(c => c.AllowedScopes)
                        .Single(c => c.ClientId == client.ClientId);

                    if (clientEntity.AllowedScopes == null && client.AllowedScopes.Count > 0)
                    {
                        clientEntity.AllowedScopes = client.AllowedScopes.Select(s => new IdentityServer4.EntityFramework.Entities.ClientScope
                        {
                            Scope = s
                        }).ToList();
                    }
                    else if (clientEntity.AllowedScopes != null)
                    {
                        var scopesNotSaved = client.AllowedScopes.Where(s => !clientEntity.AllowedScopes.Any(cs => cs.Scope == s));
                        if (scopesNotSaved.Count() > 0)
                        {
                            clientEntity.AllowedScopes
                                .AddRange(scopesNotSaved.Select(s => new IdentityServer4.EntityFramework.Entities.ClientScope
                                {
                                    Scope = s
                                }));
                        }
                    }
                }
            }
            configurationContext.SaveChanges();
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

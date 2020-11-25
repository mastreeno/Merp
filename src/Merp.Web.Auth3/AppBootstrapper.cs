using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Merp.Web.Auth.Configuration;
using Merp.Web.Site.Models;
using Merp.Web.Auth.WorkerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            Services.AddScoped<AccountControllerWorkerServices>();
            Services.AddScoped<ManageControllerWorkerServices>();
        }

        private void ConfigureIdentityServer()
        {
            var identityServerBuilder = Services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.UserInteraction.LoginUrl = "/Account/Login";
                    options.UserInteraction.LogoutUrl = "/Account/Logout";
                    // options.IssuerUri = "";
                    // options.PublicOrigin = "";
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(
                        Configuration.GetConnectionString("IdentityServerConfigurationConnection"),
                        sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(
                        Configuration.GetConnectionString("IdentityServerConfigurationConnection"),
                        sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
                })
                .AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment())
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }
        }

        private void SetupIdentityServerConfigurationData()
        {
            var identityServerConfiguration = Services.BuildServiceProvider().GetService<IdentityServerConfiguration>();
            
            using (var configurationContext = Services.BuildServiceProvider().GetService<ConfigurationDbContext>())
            {
                foreach (var apiResource in identityServerConfiguration.GetApiResources())
                {
                    if (!configurationContext.ApiResources.Any(r => r.Name == apiResource.Name))
                    {
                        configurationContext.Add(apiResource.ToEntity());
                    }
                }

                foreach (var identityResource in identityServerConfiguration.GetIdentityResources())
                {
                    if (!configurationContext.IdentityResources.Any(i => i.Name == identityResource.Name))
                    {
                        configurationContext.Add(identityResource.ToEntity());
                    }
                }

                var clients = identityServerConfiguration.GetClients();
                foreach (var client in clients)
                {
                    if (!configurationContext.Clients.Any(c => c.ClientId == client.ClientId))
                    {
                        configurationContext.Add(client.ToEntity());
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
        }
    }
}

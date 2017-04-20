using Memento.Persistence;
using Merp.Registry.CommandStack.Events;
using Merp.Registry.CommandStack.Sagas;
using Merp.Registry.CommandStack.Services;
using Merp.Registry.QueryStack.Denormalizers;
using Merp.Web.Site.Areas.Registry.WorkerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Rebus.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry
{
    public class RegistryBoundedContextConfigurator : AppBootstrapper.BoundedContextConfigurator
    {
        public RegistryBoundedContextConfigurator(IConfigurationRoot configuration, IServiceCollection services) : base(configuration, services)
        {

        }

        protected override void ConfigureEventStore()
        {
            if (Environment.IsDevelopment() || Environment.IsOnPremises())
            {
                var mongoDbConnectionString = Configuration.GetConnectionString("Merp-Registry-EventStore");
                var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
                var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
                Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
                Services.AddTransient<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();
                Services.AddTransient<IRepository, Memento.Persistence.Repository>();
            }
            else if (Environment.IsAzure())
            {
                var mongoDbConnectionString = Configuration.GetConnectionString("Merp-Registry-EventStore");
                var mongoDbUrl = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString);
                MongoClientSettings settings = new MongoClientSettings();
                settings.Server = new MongoServerAddress(mongoDbUrl.Server.Host, mongoDbUrl.Server.Port);
                settings.UseSsl = true;
                settings.SslSettings = new SslSettings();
                settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

                MongoIdentity identity = new MongoInternalIdentity(mongoDbUrl.DatabaseName, mongoDbUrl.Username);
                MongoIdentityEvidence evidence = new PasswordEvidence(mongoDbUrl.Password);

                settings.Credentials = new List<MongoCredential>()
                    {
                        new MongoCredential("SCRAM-SHA-1", identity, evidence)
                    };

                MongoClient client = new MongoClient(settings);
                Services.AddSingleton(client.GetDatabase(mongoDbUrl.DatabaseName));
                Services.AddTransient<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();
                Services.AddTransient<IRepository, Memento.Persistence.Repository>();
            }
            else
            {
                throw new InvalidOperationException("Unknown execution environment");
            }
        }

        protected override void SubscribeEvents()
        {
            //Events
            Bus.Subscribe<CompanyNameChangedEvent>();
            Bus.Subscribe<CompanyRegisteredEvent>();
            Bus.Subscribe<PersonRegisteredEvent>();
            Bus.Subscribe<LegalAddressSetForPartyEvent>();
            Bus.Subscribe<ShippingAddressSetForPartyEvent>();
            Bus.Subscribe<BillingAddressSetForPartyEvent>();
        }

        protected override void RegisterDenormalizers()
        {
            //Denormalizers
            Services.AutoRegisterHandlersFromAssemblyOf<CompanyDenormalizer>();
        }

        protected override void RegisterHandlers()
        {
            //Handlers
        }

        protected override void RegisterSagas()
        {
            //Sagas
            Services.AutoRegisterHandlersFromAssemblyOf<CompanySaga>();
        }

        protected override void RegisterServices()
        {
            Services.AddScoped<IDefaultCountryResolver, DefaultCountryResolver>();
        }

        protected override void RegisterTypes()
        {
            //Types
            var readModelConnectionString = Configuration.GetConnectionString("Merp-Registry-ReadModel");

            Services.AddScoped<Merp.Registry.QueryStack.RegistryDbContext>((s) => new Merp.Registry.QueryStack.RegistryDbContext(readModelConnectionString));
            Services.AddScoped<Merp.Registry.QueryStack.IDatabase, Merp.Registry.QueryStack.Database>((s) => new Merp.Registry.QueryStack.Database(readModelConnectionString));
        }

        protected override void RegisterWorkerServices()
        {
            //Worker Services
            Services.AddScoped<ApiControllerWorkerServices>();
            Services.AddScoped<PersonControllerWorkerServices>();
            Services.AddScoped<PartyControllerWorkerServices>();
            Services.AddScoped<CompanyControllerWorkerServices>();
        }
    }
}

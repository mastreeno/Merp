using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.ServiceProvider;
using MementoFX.Persistence;
using Merp.Registry.CommandStack.Events;
using Merp.Registry.CommandStack.Sagas;
using Merp.Registry.CommandStack.Services;
using Merp.Registry.QueryStack.Denormalizers;
using Merp.Registry.Web.Core.Configuration;
using Merp.Web;

namespace Merp.Registry.Web
{
    public class RegistryBoundedContextConfigurator : BoundedContextConfigurator
    {
        public readonly IBoundedContextConfigurationProvider BoundedContextConfigurationProvider;

        public RegistryBoundedContextConfigurator(IConfiguration configuration, IServiceCollection services, IBoundedContextConfigurationProvider boundedContextConfigurationProvider) : base(configuration, services)
        {
            BoundedContextConfigurationProvider = boundedContextConfigurationProvider ?? throw new ArgumentNullException(nameof(boundedContextConfigurationProvider));
        }

        protected override void ConfigureEventStore()
        {
            var mongoDbConnectionString = BoundedContextConfigurationProvider.GetEventStoreConnectionString();
            var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
            var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
            Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
            Services.AddTransient<IEventStore, MementoFX.Persistence.MongoDB.MongoDbEventStore>();
            Services.AddTransient<IRepository, MementoFX.Persistence.Repository>();
        }

        protected override void SubscribeEvents()
        {
            //Events
            Bus.Subscribe<CompanyNameChangedEvent>();
            Bus.Subscribe<CompanyRegisteredEvent>();
            Bus.Subscribe<PersonRegisteredEvent>();
            Bus.Subscribe<PartyLegalAddressChangedEvent>();
            Bus.Subscribe<PartyShippingAddressChangedEvent>();
            Bus.Subscribe<PartyBillingAddressChangedEvent>();
            Bus.Subscribe<CompanyAdministrativeContactAssociatedEvent>();
            Bus.Subscribe<CompanyMainContactAssociatedEvent>();
            Bus.Subscribe<ContactInfoSetForPartyEvent>();
            Bus.Subscribe<PartyUnlistedEvent>();
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
            var readModelConnectionString = BoundedContextConfigurationProvider.GetReadModelConnectionString();
            Services.AddDbContext<Merp.Registry.QueryStack.RegistryDbContext>(options => options.UseSqlServer(readModelConnectionString));
            Services.AddScoped<Merp.Registry.QueryStack.IDatabase, Merp.Registry.QueryStack.Database>();
        }

        protected override void RegisterAclServices()
        {
            //Acl Services
            Services.AddScoped<Acl.RegistryResolutionServices.Resolver>();
        }
    }
}

using Memento.Messaging;
using Memento.Messaging.Rebus;
using Memento.Persistence;
using Merp.Registry.CommandStack.Events;
using Merp.Registry.CommandStack.Sagas;
using Merp.Registry.QueryStack.Denormalizers;
using Microsoft.Practices.Unity;
using MongoDB.Driver;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CloudService.Worker
{
    class Bootstrapper
    {
        public IUnityContainer Container { get; private set; }

        public void Initialise()
        {
            Container = new UnityContainer();
            ConfigureBus();
            ConfigureEventDispatcher();
            ConfigurePersistence();
            new RegistryBoundedContext(Container).Configure();
        }

        private void ConfigureBus()
        {
            var container = new UnityContainer();
            var config = Rebus.Config.Configure.With(new UnityContainerAdapter(Container))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<CompanySaga>(ConfigurationManager.AppSettings["Rebus:QueueName"])
                )
                .Subscriptions(s => s.StoreInSqlServer(ConfigurationManager.AppSettings["Rebus:Subscriptions:ConnectionString"], ConfigurationManager.AppSettings["Rebus:Subscriptions:TableName"], isCentralized: true))
                .Sagas(s => s.StoreInSqlServer(ConfigurationManager.AppSettings["Rebus:Sagas:ConnectionString"], ConfigurationManager.AppSettings["Rebus:Sagas:MessagesTableName"], ConfigurationManager.AppSettings["Rebus:Sagas:IndexesTableName"]))
                .Timeouts(t => t.StoreInSqlServer(ConfigurationManager.AppSettings["Rebus:Timeouts:ConnectionString"], ConfigurationManager.AppSettings["Rebus:Timeouts:TableName"], true))
                .Transport(t => t.UseAzureServiceBus(ConfigurationManager.AppSettings["Rebus:ServiceBusConnectionString"], ConfigurationManager.AppSettings["Rebus:QueueName"], AzureServiceBusMode.Basic));
            var bus = config.Start();
            Container.RegisterInstance<IBus>(bus);
        }

        public void ConfigureEventDispatcher()
        {
            Container.RegisterType<IEventDispatcher, RebusEventDispatcher>();
        }

        private void ConfigurePersistence()
        {
            var mongoDbConnectionString = ConfigurationManager.ConnectionStrings["Merp-Registry-EventStore"].ConnectionString;
            var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
            var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
            Container.RegisterInstance(mongoClient.GetDatabase(mongoDbDatabaseName));
            Container.RegisterType<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();
            Container.RegisterType<IRepository, Memento.Persistence.Repository>();
        }

        public class RegistryBoundedContext
        {
            public IBus Bus { get; set; }
            public IUnityContainer Container { get; set; }

            public RegistryBoundedContext(IUnityContainer container)
            {
                if (container == null)
                    throw new ArgumentNullException(nameof(container));

                Bus = container.Resolve<IBus>();
                Container = container;
            }

            public void Configure()
            {
                RegisterReadModel();
                RegisterDenormalizers();
                RegisterHandlers();
                RegisterSagas();
                RegisterServices();
                SubscribeEvents();
            }

            private void RegisterReadModel()
            {
                Container.RegisterType<Merp.Registry.QueryStack.RegistryDbContext>(new InjectionConstructor(ConfigurationManager.ConnectionStrings["Merp-Registry-ReadModel"].ConnectionString));
                Container.RegisterType<Merp.Registry.QueryStack.IDatabase, Merp.Registry.QueryStack.Database>();
            }

            private void RegisterHandlers()
            {

            }

            private void RegisterDenormalizers()
            {
                Container.RegisterTypes(AllClasses.FromAssemblies(typeof(CompanyDenormalizer).Assembly),
                    WithMappings.FromAllInterfaces,
                    WithName.TypeName,
                    WithLifetime.Transient
                    );
            }

            private void RegisterSagas()
            {
                Container.RegisterTypes(AllClasses.FromAssemblies(typeof(CompanySaga).Assembly),
                    WithMappings.FromAllInterfaces,
                    WithName.TypeName,
                    WithLifetime.Transient
                    );
            }

            private void RegisterServices()
            {

            }

            private void SubscribeEvents()
            {
                Bus.Subscribe<CompanyNameChangedEvent>();
                Bus.Subscribe<CompanyRegisteredEvent>();
                Bus.Subscribe<PersonRegisteredEvent>();
                Bus.Subscribe<PartyLegalAddressChangedEvent>();
                Bus.Subscribe<PartyShippingAddressChangedEvent>();
                Bus.Subscribe<PartyBillingAddressChangedEvent>();
                Bus.Subscribe<CompanyAdministrativeContactAssociatedEvent>();
                Bus.Subscribe<CompanyMainContactAssociatedEvent>();
                Bus.Subscribe<ContactInfoSetForPartyEvent>();
            }
        }
    }
}

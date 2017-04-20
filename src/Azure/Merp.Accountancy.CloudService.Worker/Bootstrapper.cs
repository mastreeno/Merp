using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.QueryStack.Denormalizers;
using Microsoft.Practices.Unity;
using Rebus.Bus;
using Rebus.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebus.Routing.TypeBased;
using Merp.Accountancy.CommandStack.Sagas;
using Rebus.Config;
using MongoDB.Driver;
using Memento.Persistence;
using System.Security.Authentication;
using Memento.Messaging;
using Merp.Accountancy.CommandStack.Services;

namespace Merp.Accountancy.CloudService.Worker
{
    class Bootstrapper
    {
        public IUnityContainer Container { get; private set; }

        public void Initialise()
        {
            Container = new UnityContainer();
            ConfigureBus();
            ConfigurePersistence();
            new AccountancyBoundedContext(Container).Configure();
        }

        private void ConfigureBus()
        {
            var container = new UnityContainer();
            var config = Rebus.Config.Configure.With(new UnityContainerAdapter(Container))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<IncomingInvoiceSaga>(ConfigurationManager.AppSettings["Rebus:QueueName"])
                )
                .Subscriptions(s => s.StoreInSqlServer(ConfigurationManager.AppSettings["Rebus:Subscriptions:ConnectionString"], ConfigurationManager.AppSettings["Rebus:Subscriptions:TableName"], isCentralized: true))
                .Sagas(s => s.StoreInSqlServer(ConfigurationManager.AppSettings["Rebus:Sagas:ConnectionString"], ConfigurationManager.AppSettings["Rebus:Sagas:MessagesTableName"], ConfigurationManager.AppSettings["Rebus:Sagas:IndexesTableName"]))
                .Timeouts(t => t.StoreInSqlServer(ConfigurationManager.AppSettings["Rebus:Timeouts:ConnectionString"], ConfigurationManager.AppSettings["Rebus:Timeouts:TableName"], true))
                .Transport(t => t.UseAzureServiceBus(ConfigurationManager.AppSettings["Rebus:ServiceBusConnectionString"], ConfigurationManager.AppSettings["Rebus:QueueName"], AzureServiceBusMode.Basic));
            var bus = config.Start();
            Container.RegisterInstance<IBus>(bus);
        }

        private void ConfigurePersistence()
        {
            var mongoDbConnectionString = ConfigurationManager.ConnectionStrings["Merp-Accountancy-EventStore"].ConnectionString;
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
            Container.RegisterInstance(client.GetDatabase(mongoDbUrl.DatabaseName));
            Container.RegisterType<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>(new InjectionConstructor(typeof(IMongoDatabase), typeof(IEventDispatcher)));
            Container.RegisterType<IRepository, Memento.Persistence.Repository>();
        }

        public class AccountancyBoundedContext
        {
            public IBus Bus { get; set; }
            public IUnityContainer Container { get; set; }

            public AccountancyBoundedContext(IUnityContainer container)
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
                Container.RegisterType<Merp.Accountancy.QueryStack.IDatabase, Merp.Accountancy.QueryStack.Database>();
            }

            private void RegisterHandlers()
            {

            }

            private void RegisterDenormalizers()
            {
                Container.RegisterTypes(AllClasses.FromAssemblies(typeof(FixedPriceJobOrderDenormalizer).Assembly),
                    WithMappings.FromAllInterfaces,
                    WithName.TypeName,
                    WithLifetime.Transient
                    );
            }

            private void RegisterSagas()
            {
                Container.RegisterTypes(AllClasses.FromAssemblies(typeof(FixedPriceJobOrderSaga).Assembly),
                    WithMappings.FromAllInterfaces,
                    WithName.TypeName,
                    WithLifetime.Transient
                    );
            }

            private void RegisterServices()
            {
                Container.RegisterType<IJobOrderNumberGenerator, JobOrderNumberGenerator>();
                Container.RegisterType<IOutgoingInvoiceNumberGenerator, OutgoingInvoiceNumberGenerator>();
            }

            private void SubscribeEvents()
            {
                Bus.Subscribe<FixedPriceJobOrderCompletedEvent>();
                Bus.Subscribe<FixedPriceJobOrderExtendedEvent>();
                Bus.Subscribe<FixedPriceJobOrderRegisteredEvent>();
                Bus.Subscribe<IncomingInvoiceLinkedToJobOrderEvent>();
                Bus.Subscribe<IncomingInvoiceRegisteredEvent>();
                Bus.Subscribe<IncomingInvoiceExpiredEvent>();
                Bus.Subscribe<IncomingInvoicePaidEvent>();
                Bus.Subscribe<OutgoingInvoiceIssuedEvent>();
                Bus.Subscribe<OutgoingInvoiceLinkedToJobOrderEvent>();
                Bus.Subscribe<OutgoingInvoicePaidEvent>();
                Bus.Subscribe<OutgoingInvoiceExpiredEvent>();
                Bus.Subscribe<TimeAndMaterialJobOrderCompletedEvent>();
                Bus.Subscribe<TimeAndMaterialJobOrderExtendedEvent>();
                Bus.Subscribe<TimeAndMaterialJobOrderRegisteredEvent>();
            }
        }
    }
}

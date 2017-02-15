using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.CommandStack.Sagas;
using Merp.Accountancy.CommandStack.Services;
using Merp.Accountancy.QueryStack.Denormalizers;
using Merp.Registry.CommandStack.Events;
using Merp.Registry.CommandStack.Sagas;
using Merp.Registry.QueryStack.Denormalizers;
using Microsoft.Practices.Unity;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denormalizers
{
    class Bootstrapper
    {
        public IUnityContainer Container { get; private set; }

        public void Initialise()
        {
            Container = new UnityContainer();
            ConfigureBus();
            new AccountancyBoundedContext(Container).Configure();
            new RegistryBoundedContext(Container).Configure();
        }

        private void ConfigureBus()
        {
            var container = new UnityContainer();
            var config = Rebus.Config.Configure.With(new UnityContainerAdapter(Container))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<IncomingInvoiceSaga>(ConfigurationManager.AppSettings["Rebus:QueueName"])
                    .MapAssemblyOf<CompanySaga>(ConfigurationManager.AppSettings["Rebus:QueueName"])
                )
                .Subscriptions(s => s.StoreInSqlServer(ConfigurationManager.AppSettings["Rebus:Subscriptions:ConnectionString"], ConfigurationManager.AppSettings["Rebus:Subscriptions:TableName"], isCentralized: true))
                .Sagas(s => s.StoreInSqlServer(ConfigurationManager.AppSettings["Rebus:Sagas:ConnectionString"], ConfigurationManager.AppSettings["Rebus:Sagas:MessagesTableName"], ConfigurationManager.AppSettings["Rebus:Sagas:IndexesTableName"]))
                .Timeouts(t => t.StoreInSqlServer(ConfigurationManager.AppSettings["Rebus:Timeouts:ConnectionString"], ConfigurationManager.AppSettings["Rebus:Timeouts:TableName"], true))
                .Transport(t => t.UseAzureServiceBus(ConfigurationManager.AppSettings["Rebus:ServiceBusConnectionString"], ConfigurationManager.AppSettings["Rebus:QueueName"], Rebus.AzureServiceBus.Config.AzureServiceBusMode.Basic));
            var bus = config.Start();
            Container.RegisterInstance<IBus>(bus);
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

            }

            private void RegisterServices()
            {

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

            }

            private void RegisterServices()
            {

            }

            private void SubscribeEvents()
            {
                Bus.Subscribe<CompanyNameChangedEvent>();
                Bus.Subscribe<CompanyRegisteredEvent>();
                Bus.Subscribe<PersonRegisteredEvent>();
            }
        }
    }
}

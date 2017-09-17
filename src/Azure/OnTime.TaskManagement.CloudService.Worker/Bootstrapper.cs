using Microsoft.Practices.Unity;
using Rebus.Bus;
using Rebus.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebus.Routing.TypeBased;
using OnTime.TaskManagement.CommandStack.Sagas;
using System.Configuration;
using Rebus.Config;
using MongoDB.Driver;
using System.Security.Authentication;
using Memento.Persistence;
using Memento.Messaging;
using OnTime.TaskManagement.QueryStack.Denormalizers;
using OnTime.TaskManagement.CommandStack.Events;
using OnTime.TaskManagement.QueryStack;
using Memento.Messaging.Rebus;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace OnTime.TaskManagement.CloudService.Worker
{
    public class Bootstrapper
    {
        public IUnityContainer Container { get; private set; }

        public void Initialise()
        {
            Container = new UnityContainer();
            ConfigureBus();
            ConfigureEventDispatcher();
            ConfigurePersistence();
            new OnTimeBoundedContext(Container).Configure();
        }

        private void ConfigureBus()
        {
            var container = new UnityContainer();
            var config = Rebus.Config.Configure.With(new UnityContainerAdapter(Container))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<TaskLifecycleSaga>(ConfigurationManager.AppSettings["Rebus:QueueName"])
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
            var mongoDbConnectionString = RoleEnvironment.GetConfigurationSettingValue("OnTime-TaskManagement-EventStore");
            var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
            var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
            Container.RegisterInstance(mongoClient.GetDatabase(mongoDbDatabaseName));
            Container.RegisterType<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();
            Container.RegisterType<IRepository, Memento.Persistence.Repository>();
        }

        public class OnTimeBoundedContext
        {
            public IBus Bus { get; set; }

            public IUnityContainer Container { get; set; }

            public OnTimeBoundedContext(IUnityContainer container)
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
                Container.RegisterType<TaskManagementDbContext>(new InjectionConstructor(RoleEnvironment.GetConfigurationSettingValue("OnTime-TaskManagement-ReadModel")));
                Container.RegisterType<IDatabase, Database>();
            }

            private void RegisterHandlers()
            {

            }

            private void RegisterDenormalizers()
            {
                Container.RegisterTypes(AllClasses.FromAssemblies(typeof(TaskEvents).Assembly),
                    WithMappings.FromAllInterfaces,
                    WithName.TypeName,
                    WithLifetime.Transient
                    );
            }

            private void RegisterSagas()
            {
                Container.RegisterTypes(AllClasses.FromAssemblies(typeof(TaskLifecycleSaga).Assembly),
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
                Bus.Subscribe<DueDateRemovedFromTaskEvent>();
                Bus.Subscribe<DueDateSetForTaskEvent>();
                Bus.Subscribe<TaskCancelledEvent>();
                Bus.Subscribe<TaskCompletedEvent>();
                Bus.Subscribe<TaskCreatedEvent>();
                Bus.Subscribe<TaskReactivatedEvent>();
                Bus.Subscribe<TaskRenamedEvent>();
            }
        }
    }
}

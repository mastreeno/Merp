using Memento.Messaging;
using Memento.Messaging.Rebus;
using Memento.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnTime.TaskManagement.CommandStack.Sagas;
using OnTime.TaskManagement.QueryStack;
using OnTime.TaskManagement.QueryStack.Denormalizers;
using OnTime.TaskManagement.QueryStack.Model;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using System;

namespace OnTime.Web.Site
{
    public class AppBootstrapper
    {
        public IServiceCollection Services { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }

        public AppBootstrapper(IConfigurationRoot configuration, IServiceCollection services)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            Configuration = configuration;
            Services = services;
        }

        public void RegisterSagas()
        {
            Services.AutoRegisterHandlersFromAssemblyOf<TaskLifecycleSaga>();
        }

        public void ConfigureReadModel()
        {
            var readModelConnectionString = Configuration.GetConnectionString("OnTime-TaskManagement-ReadModel");
            Services.AddScoped<TaskManagementDbContext>((s) => new TaskManagementDbContext(readModelConnectionString));
            Services.AddScoped<IDatabase, Database>((s) => new Database(readModelConnectionString));
        }

        public void RegisterDenormalizers()
        {
            Services.AutoRegisterHandlersFromAssemblyOf<TaskEvents>();
        }

        public void RegisterHandlers()
        {
            
        }

        public void ConfigureBus()
        {
            Rebus.Config.Configure.With(new NetCoreServiceCollectionContainerAdapter(Services))
                //.Options(o => o.EnableFleetManager("https://fm-test.azurewebsites.net", "Mdh6EuJoKh08UNfqT4mrPE8eBmqcUZlRjAO+Hqm1NHw8qraNLSQv5Og2DjvcNTFxEtQiSeahAgWXaeOhn0LijPgSL1ExsaKKyivUuawl/gEVvxWiSQVS9ukQPozWfE8wcAYqW2vdU3o6uu5i373PYls9MaflZeRDRb4B5n0ZRfrK7RTsXCLPP0gBE36brw9ur1VTpNNsSjNb1OeuVvJv1UZCAtxzpAAMKo9rCn8q56wItiBxf2udkC/eBFIMaP4bac8kx4JTzfhkPN7XVFHffasmLH8MgG+8VxLqQRyiDWlQ38iDoNvWBeoL1sG43+T2rtOe99jnLGRWUJRosjgkKn9hkyRr9hidUiDCSDRD8zvCbo/FquHtvFOH66zBB5/Yx8xy2VL2a/N/ieEFJ2Z0YBI+aW56YsDavR5kwBIn49aXWcSkPWDiBQcKS1/VMCuUWWPf/U0Wf5B2qgVCqIuzhEY6zrHQ8O7SdfarggK8yB8="))
                .Logging(l => l.Trace())
                .Transport(t => t.UseMsmq(Configuration["Rebus:QueueName"]))
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<TaskLifecycleSaga>(Configuration["Rebus:QueueName"])
                )
                .Subscriptions(s => s.StoreInSqlServer(Configuration["Rebus:Subscriptions:ConnectionString"], Configuration["Rebus:Subscriptions:TableName"]))
                .Sagas(s => s.StoreInSqlServer(Configuration["Rebus:Sagas:ConnectionString"], Configuration["Rebus:Sagas:MessagesTableName"], Configuration["Rebus:Sagas:IndexesTableName"]))
                .Timeouts(t => t.StoreInSqlServer(Configuration["Rebus:Timeouts:ConnectionString"], Configuration["Rebus:Timeouts:TableName"], true))
                .Start();
            Services.AddTransient<IEventDispatcher, RebusEventDispatcher>();
        }

        public void ConfigureEventStore()
        {
            var mongoDbConnectionString = Configuration.GetConnectionString("OnTime-TaskManagement-EventStore");
            var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
            var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
            Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));

            Services.AddTransient<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();

            Services.AddTransient<IRepository, Memento.Persistence.Repository>();
        }
    }
}

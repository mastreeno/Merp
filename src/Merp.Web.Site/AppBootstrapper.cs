using System;
using Memento.Messaging;
using Memento.Messaging.Rebus;
using Merp.Accountancy.CommandStack.Sagas;
using Merp.Registry.CommandStack.Sagas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Microsoft.AspNetCore.Hosting;
using Merp.Web.Site.Areas.Accountancy;
using Merp.Web.Site.Areas.Registry;
using Merp.Web.Site.Services.Rebus;

namespace Merp.Web.Site
{
    public class AppBootstrapper
    {
        public IServiceCollection Services { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }
        public IHostingEnvironment Environment { get; set; }

        public AppBootstrapper(IConfigurationRoot configuration, IServiceCollection services)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            Configuration = configuration;
            Services = services;
            Environment = Services.BuildServiceProvider().GetService<IHostingEnvironment>();
        }

        public void Configure()
        {
            var env = Services.BuildServiceProvider().GetService<IHostingEnvironment>();
            ConfigureBus();
            var bus = Services.BuildServiceProvider().GetService<IBus>();
            new AccountancyBoundedContextConfigurator(Configuration, Services).Configure();
            new RegistryBoundedContextConfigurator(Configuration, Services).Configure();
        }

        private void ConfigureBus()
        {
            var config = Rebus.Config.Configure.With(new NetCoreServiceCollectionContainerAdapter(Services))
                //.Options(o => {
                //    o.SetNumberOfWorkers(1);
                //    o.SetMaxParallelism(50);
                //})
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<IncomingInvoiceSaga>(Configuration["Rebus:QueueName"])
                    .MapAssemblyOf<CompanySaga>(Configuration["Rebus:QueueName"])
                )
                .Subscriptions(s => s.StoreInSqlServer(Configuration["Rebus:Subscriptions:ConnectionString"], Configuration["Rebus:Subscriptions:TableName"], isCentralized: true))
                .Sagas(s => s.StoreInSqlServer(Configuration["Rebus:Sagas:ConnectionString"], Configuration["Rebus:Sagas:MessagesTableName"], Configuration["Rebus:Sagas:IndexesTableName"]))
                .Timeouts(t => t.StoreInSqlServer(Configuration["Rebus:Timeouts:ConnectionString"], Configuration["Rebus:Timeouts:TableName"], true));
            if (Environment.IsDevelopment() || Environment.IsOnPremises())
            {
                config.Transport(t => t.UseMsmq(Configuration["Rebus:QueueName"]));
            }
            else if (Environment.IsAzureCloudServices() || Environment.IsAzureCosmosDB() || Environment.IsAzureMongoDB())
            {
                config.Transport(t => t.UseAzureServiceBus(Configuration["Rebus:ServiceBusConnectionString"], Configuration["Rebus:QueueName"], AzureServiceBusMode.Basic));
            }
            else
            {
                throw new InvalidOperationException("Unknown execution environment");
            }
            var bus = config.Start();
            Services.AddSingleton(bus);
            Services.AddTransient<IEventDispatcher, RebusEventDispatcher>();
        }

        public abstract class BoundedContextConfigurator
        {
            public IBus Bus { get; private set; }
            public IHostingEnvironment Environment { get; private set; }
            public IServiceCollection Services { get; private set; }
            public IConfigurationRoot Configuration { get; private set; }

            protected abstract void RegisterDenormalizers();
            protected abstract void RegisterHandlers();
            protected abstract void RegisterSagas();
            protected abstract void RegisterServices();
            protected abstract void SubscribeEvents();
            protected abstract void RegisterTypes();
            protected abstract void RegisterWorkerServices();
            protected abstract void RegisterAclServices();
            protected abstract void ConfigureEventStore();

            public BoundedContextConfigurator(IConfigurationRoot configuration, IServiceCollection services)
            {               
                Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
                Services = services ?? throw new ArgumentNullException(nameof(services));
                Bus = services.BuildServiceProvider().GetService<IBus>();
                Environment = services.BuildServiceProvider().GetService<IHostingEnvironment>();
            }

            public void Configure()
            {
                if (Environment.IsDevelopment() || 
                    Environment.IsOnPremises() || 
                    Environment.IsAzureCosmosDB() || 
                    Environment.IsAzureMongoDB())
                {
                    RegisterDenormalizers();
                    RegisterHandlers();
                    RegisterSagas();
                    RegisterServices();
                    SubscribeEvents();
                }
                RegisterTypes();
                RegisterWorkerServices();
                RegisterAclServices();
                ConfigureEventStore();
            }
        }
    }

    static class EnvironmentExtensions
    {
        public static bool IsAzureCloudServices(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Contains("AzureCloudServices");
        }
        public static bool IsAzureCosmosDB(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Contains("AzureCosmosDB");
        }
        public static bool IsAzureMongoDB(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Contains("AzureMongoDB");
        }
        public static bool IsOnPremises(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Contains("OnPremises");
        }
    }
}
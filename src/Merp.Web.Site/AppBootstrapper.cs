using System;
using MementoFX.Messaging;
using MementoFX.Messaging.Rebus;
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
using Merp.Web.Site.Areas.OnTime;
using OnTime.TaskManagement.CommandStack.Sagas;
using Merp.Web.Site.Services.Rebus;
using Merp.ProjectManagement.Web.Areas.PM;

namespace Merp.Web.Site
{
    public class AppBootstrapper
    {
        public IServiceCollection Services { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }
        public IHostingEnvironment Environment { get; set; }

        public AppBootstrapper(IConfigurationRoot configuration, IServiceCollection services)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = Services.BuildServiceProvider().GetService<IHostingEnvironment>();
        }

        public void Configure()
        {
            var env = Services.BuildServiceProvider().GetService<IHostingEnvironment>();
            ConfigureBus();
            var bus = Services.BuildServiceProvider().GetService<IBus>();
            new AccountancyBoundedContextConfigurator(Configuration, Services).Configure();
            new OnTimeBoundedContextConfigurator(Configuration, Services).Configure();
            new ProjectManagementBoundedContextConfigurator(Configuration, Services).Configure();
            new RegistryBoundedContextConfigurator(Configuration, Services).Configure();
        }

        private void ConfigureBus()
        {
            var config = Rebus.Config.Configure.With(new NetCoreServiceCollectionContainerAdapter(Services))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<IncomingInvoiceSaga>(Configuration["Rebus:QueueName"])
                    .MapAssemblyOf<CompanySaga>(Configuration["Rebus:QueueName"])
                    .MapAssemblyOf<TaskLifecycleSaga>(Configuration["Rebus:QueueName"])
                )
                .Subscriptions(s => s.StoreInSqlServer(Configuration["Rebus:Subscriptions:ConnectionString"], Configuration["Rebus:Subscriptions:TableName"], isCentralized: true))
                .Sagas(s => s.StoreInSqlServer(Configuration["Rebus:Sagas:ConnectionString"], Configuration["Rebus:Sagas:MessagesTableName"], Configuration["Rebus:Sagas:IndexesTableName"]))
                .Timeouts(t => t.StoreInSqlServer(Configuration["Rebus:Timeouts:ConnectionString"], Configuration["Rebus:Timeouts:TableName"], true));
            if (Environment.IsDevelopment() || Environment.IsOnPremises())
            {
                config.Transport(t => t.UseMsmq(Configuration["Rebus:QueueName"]));
            }
            else if (Environment.IsAzureCosmosDB() || Environment.IsAzureMongoDB())
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



    }

    
}
using MementoFX.Messaging;
using MementoFX.Messaging.Rebus;
using Merp.Accountancy.CommandStack.Sagas;
using Merp.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public
{
    public class AppBootstrapper
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; set; }

        public IServiceCollection Services { get; }

        public AppBootstrapper(IConfiguration configuration, IServiceCollection services)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = Services.BuildServiceProvider().GetService<IWebHostEnvironment>();
        }

        public void Configure()
        {
            ConfigureBus();
            var bus = Services.BuildServiceProvider().GetService<IBus>();
            new AccountancyPublicBoundedContextConfigurator(Configuration, Services).Configure();
        }

        private void ConfigureBus()
        {
            var config = Rebus.Config.Configure.With(new NetCoreServiceCollectionContainerAdapter(Services))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<IncomingInvoiceSaga>(Configuration["Rebus:QueueName"])
                )
                .Subscriptions(s => s.StoreInSqlServer(Configuration["Rebus:Subscriptions:ConnectionString"], Configuration["Rebus:Subscriptions:TableName"], isCentralized: true))
                .Sagas(s => s.StoreInSqlServer(Configuration["Rebus:Sagas:ConnectionString"], Configuration["Rebus:Sagas:MessagesTableName"], Configuration["Rebus:Sagas:IndexesTableName"]));
            if (Environment.IsDevelopment() || Environment.IsOnPremises())
            {
                config.Transport(t => t.UseSqlServer(Configuration["Rebus:Transport:ConnectionString"], Configuration["Rebus:QueueName"]));
            }
            else if (Environment.IsAzureCosmosDB() || Environment.IsAzureMongoDB())
            {
                config.Transport(t => t.UseAzureServiceBus(Configuration["Rebus:ServiceBusConnectionString"], Configuration["Rebus:QueueName"]));
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

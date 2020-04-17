using MementoFX.Messaging;
using MementoFX.Messaging.Rebus;
using Merp.Registry.CommandStack.Sagas;
using Merp.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;

namespace Merp.Registry.Web.Api.Internal
{
    public class AppBootstrapper
    {
        public AppBootstrapper(IConfiguration configuration, IServiceCollection services)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = Services.BuildServiceProvider().GetService<IWebHostEnvironment>();
        }

        public IConfiguration Configuration { get; }
        public IServiceCollection Services { get; }
        public IWebHostEnvironment Environment { get; set; }

        public void Configure()
        {
            ConfigureBus();
            var bus = Services.BuildServiceProvider().GetService<IBus>();
            new RegistryInternalBoundedContextConfigurator(Configuration, Services).Configure();
        }

        private void ConfigureBus()
        {
            var config = Rebus.Config.Configure.With(new NetCoreServiceCollectionContainerAdapter(Services))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<CompanySaga>(Configuration["Rebus:QueueName"])
                )
                .Sagas(s => s.StoreInSqlServer(Configuration["Rebus:Sagas:ConnectionString"], Configuration["Rebus:Sagas:MessagesTableName"], Configuration["Rebus:Sagas:IndexesTableName"]));
            if (Environment.IsDevelopment() || Environment.IsOnPremises())
            {
                config.Subscriptions(s => s.StoreInSqlServer(Configuration["Rebus:Subscriptions:ConnectionString"], Configuration["Rebus:Subscriptions:TableName"], isCentralized: true));
                config.Transport(t => t.UseSqlServer(Configuration["Rebus:Transport:ConnectionString"], Configuration["Rebus:QueueName"]));
            }
            else if (Environment.IsAzureCosmosDB() || Environment.IsAzureMongoDB())
            {
                config.Transport(t => t.UseAzureServiceBus(Configuration["Rebus:Transport:ConnectionString"], Configuration["Rebus:QueueName"]));
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

using Amazon.SQS;
using MementoFX.Messaging;
using MementoFX.Messaging.Rebus;
using Merp.Accountancy.CommandStack.Sagas;
using Merp.Accountancy.Web.Core.Configuration;
using Merp.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;

namespace Merp.Accountancy.Web
{
    public class AppBootstrapper
    {
        public IConfiguration Configuration { get; }
        public IServiceCollection Services { get; }
        public IWebHostEnvironment Environment { get; set; }

        public readonly IBoundedContextConfigurationProvider BoundedContextConfigurationProvider;

        public AppBootstrapper(IConfiguration configuration, IServiceCollection services, IBoundedContextConfigurationProvider boundedContextConfigurationProvider)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            BoundedContextConfigurationProvider = boundedContextConfigurationProvider ?? throw new ArgumentNullException(nameof(boundedContextConfigurationProvider));
            Environment = Services.BuildServiceProvider().GetService<IWebHostEnvironment>();
        }

        public void Configure()
        {
            ConfigureBus();
            var bus = Services.BuildServiceProvider().GetService<IBus>();
            new AccountancyBoundedContextConfigurator(Configuration, Services, BoundedContextConfigurationProvider).Configure();
        }

        private void ConfigureBus()
        {
            var config = Rebus.Config.Configure.With(new NetCoreServiceCollectionContainerAdapter(Services))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<IncomingInvoiceSaga>(BoundedContextConfigurationProvider.GetRebusQueueName())
                )
                .Sagas(s => s.StoreInSqlServer(BoundedContextConfigurationProvider.GetRebusSagasConnectionString(), BoundedContextConfigurationProvider.GetRebusSagasMessagesTableName(), BoundedContextConfigurationProvider.GetRebusSagasIndexesTableName()));

            if (Environment.IsDevelopment() || Environment.IsOnPremises())
            {
                config.Subscriptions(s => s.StoreInSqlServer(BoundedContextConfigurationProvider.GetRebusSubscriptionsConnectionString(), BoundedContextConfigurationProvider.GetRebusSubscriptionsTableName(), isCentralized: true));
                var transportOptions = new SqlServerTransportOptions(BoundedContextConfigurationProvider.GetRebusTransportConnectionString());
                config.Transport(t => t.UseSqlServer(transportOptions, BoundedContextConfigurationProvider.GetRebusQueueName()));
            }
            else if (Environment.IsAWS())
            {
                var sqsConfig = new AmazonSQSConfig() { RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(Configuration["Rebus:Transport:RegionEndpoint"]) };
                config.Transport(t => t.UseAmazonSQS(Configuration["Rebus:Transport:AccessKey"], Configuration["Rebus:Transport:SecretKey"], sqsConfig, Configuration["Rebus:Transport:QueueAddress"]));
            }
            else if (Environment.IsAzure())
            {
                config.Transport(t => t.UseAzureServiceBus(BoundedContextConfigurationProvider.GetRebusTransportConnectionString(), BoundedContextConfigurationProvider.GetRebusQueueName()));
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

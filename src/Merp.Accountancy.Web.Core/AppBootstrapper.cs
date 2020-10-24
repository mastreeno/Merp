using Amazon.SQS;
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

namespace Merp.Accountancy.Web
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
            new AccountancyBoundedContextConfigurator(Configuration, Services).Configure();
        }

        private void ConfigureBus()
        {
            var config = Rebus.Config.Configure.With(new NetCoreServiceCollectionContainerAdapter(Services))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<IncomingInvoiceSaga>(Configuration["Rebus:QueueName"])
                )
                .Sagas(s => s.StoreInSqlServer(Configuration["Rebus:Sagas:ConnectionString"], Configuration["Rebus:Sagas:MessagesTableName"], Configuration["Rebus:Sagas:IndexesTableName"]));

            if (Environment.IsDevelopment() || Environment.IsOnPremises())
            {
                config.Subscriptions(s => s.StoreInSqlServer(Configuration["Rebus:Subscriptions:ConnectionString"], Configuration["Rebus:Subscriptions:TableName"], isCentralized: true));
                var transportOptions = new SqlServerTransportOptions(Configuration["Rebus:Transport:ConnectionString"]);
                config.Transport(t => t.UseSqlServer(transportOptions, Configuration["Rebus:QueueName"]));
            }
            else if (Environment.IsAWS())
            {
                var sqsConfig = new AmazonSQSConfig() { RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(Configuration["Rebus:Transport:RegionEndpoint"]) };
                config.Transport(t => t.UseAmazonSQS(Configuration["Rebus:Transport:AccessKey"], Configuration["Rebus:Transport:SecretKey"], sqsConfig, Configuration["Rebus:Transport:QueueAddress"]));
            }
            else if (Environment.IsAzure())
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

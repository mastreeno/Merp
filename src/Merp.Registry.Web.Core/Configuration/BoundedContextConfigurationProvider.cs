using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Merp.Registry.Web.Core.Configuration
{
    public class BoundedContextConfigurationProvider : IBoundedContextConfigurationProvider
    {
        public readonly IConfiguration Configuration;

        public BoundedContextConfigurationProvider(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GetEventStoreConnectionString()
        {
            return Configuration.GetConnectionString("Merp-Registry-EventStore");
        }

        public string GetReadModelConnectionString()
        {
            return Configuration.GetConnectionString("Merp-Registry-ReadModel");
        }

        public string GetRebusQueueName()
        {
            return Configuration["Rebus:QueueName"];
        }

        public string GetRebusSagasConnectionString()
        {
            return Configuration["Rebus:Sagas:ConnectionString"]; 
        }

        public string GetRebusSagasMessagesTableName()
        {
            return Configuration["Rebus:Sagas:MessagesTableName"];
        }

        public string GetRebusSagasIndexesTableName()
        {
            return Configuration["Rebus:Sagas:IndexesTableName"];
        }

        public string GetRebusSubscriptionsConnectionString()
        {
            return Configuration["Rebus:Subscriptions:ConnectionString"];
        }

        public string GetRebusSubscriptionsTableName()
        {
            return Configuration["Rebus:Subscriptions:TableName"];
        }

        public string GetRebusTransportConnectionString()
        {
            return Configuration["Rebus:Transport:ConnectionString"];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merp.Registry.Web.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Merp.Registry.Web.App.Configuration
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
            return Configuration.GetValue<string>("Modules:Registry:ConnectionStrings:EventStore");
        }

        public string GetReadModelConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Registry:ConnectionStrings:ReadModel");
        }

        public string GetRebusQueueName()
        {
            return Configuration.GetValue<string>("Modules:Registry:Rebus:QueueName");
        }

        public string GetRebusSagasConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Registry:Rebus:Sagas:ConnectionString");
        }

        public string GetRebusSagasMessagesTableName()
        {
            return Configuration.GetValue<string>("Modules:Registry:Rebus:Sagas:MessagesTableName");
        }

        public string GetRebusSagasIndexesTableName()
        {
            return Configuration.GetValue<string>("Modules:Registry:Rebus:Sagas:IndexesTableName");
        }

        public string GetRebusSubscriptionsConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Registry:Rebus:Subscriptions:ConnectionString");
        }

        public string GetRebusSubscriptionsTableName()
        {
            return Configuration.GetValue<string>("Modules:Registry:Rebus:Subscriptions:TableName");
        }

        public string GetRebusTransportConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Registry:Rebus:Transport:ConnectionString");
        }
    }
}

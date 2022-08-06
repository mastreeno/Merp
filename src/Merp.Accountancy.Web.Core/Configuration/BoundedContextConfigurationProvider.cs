using Microsoft.Extensions.Configuration;
using System;

namespace Merp.Accountancy.Web.Core.Configuration
{
    public class BoundedContextConfigurationProvider : IBoundedContextConfigurationProvider
    {
        public readonly IConfiguration Configuration;

        public BoundedContextConfigurationProvider(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GetDraftsConnectionString()
        {
            return Configuration.GetConnectionString("Merp-Accountancy-Drafts");
        }

        public string GetEventStoreConnectionString()
        {
            return Configuration.GetConnectionString("Merp-Accountancy-EventStore");
        }

        public string GetReadModelConnectionString()
        {
            return Configuration.GetConnectionString("Merp-Accountancy-ReadModel");
        }

        public string GetRebusQueueName()
        {
            return Configuration.GetValue<string>("Rebus:QueueName");
        }

        public string GetRebusSagasConnectionString()
        {
            return Configuration.GetValue<string>("Rebus:Sagas:ConnectionString");
        }

        public string GetRebusSagasIndexesTableName()
        {
            return Configuration.GetValue<string>("Rebus:Sagas:IndexesTableName");
        }

        public string GetRebusSagasMessagesTableName()
        {
            return Configuration.GetValue<string>("Rebus:Sagas:MessagesTableName");
        }

        public string GetRebusSubscriptionsConnectionString()
        {
            return Configuration.GetValue<string>("Rebus:Subscriptions:ConnectionString");
        }

        public string GetRebusSubscriptionsTableName()
        {
            return Configuration.GetValue<string>("Rebus:Subscriptions:TableName");
        }

        public string GetRebusTransportConnectionString()
        {
            return Configuration.GetValue<string>("Rebus:Transport:ConnectionString");
        }
    }
}

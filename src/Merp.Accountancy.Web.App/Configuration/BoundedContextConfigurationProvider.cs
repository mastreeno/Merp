using Merp.Accountancy.Web.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Merp.Accountancy.Web.App.Configuration
{
    public class BoundedContextConfigurationProvider : IBoundedContextConfigurationProvider
    {
        private readonly IConfiguration Configuration;

        public BoundedContextConfigurationProvider(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GetDraftsConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:ConnectionStrings:Drafts");
        }

        public string GetEventStoreConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:ConnectionStrings:EventStore");
        }

        public string GetReadModelConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:ConnectionStrings:ReadModel");
        }

        public string GetRebusQueueName()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:Rebus:QueueName");
        }

        public string GetRebusSagasConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:Rebus:Sagas:ConnectionString");
        }

        public string GetRebusSagasIndexesTableName()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:Rebus:Sagas:IndexesTableName");
        }

        public string GetRebusSagasMessagesTableName()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:Rebus:Sagas:MessagesTableName");
        }

        public string GetRebusSubscriptionsConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:Rebus:Subscriptions:ConnectionString");
        }

        public string GetRebusSubscriptionsTableName()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:Rebus:Subscriptions:TableName");
        }

        public string GetRebusTransportConnectionString()
        {
            return Configuration.GetValue<string>("Modules:Accountancy:Rebus:Transport:ConnectionString");
        }
    }
}

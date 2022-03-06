using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Core.Configuration
{
    public interface IBoundedContextConfigurationProvider
    {
        string GetReadModelConnectionString();
        string GetEventStoreConnectionString();

        string GetRebusQueueName();
        string GetRebusSagasConnectionString();
        string GetRebusSagasMessagesTableName();
        string GetRebusSagasIndexesTableName();
        string GetRebusSubscriptionsConnectionString();
        string GetRebusSubscriptionsTableName();
        string GetRebusTransportConnectionString();
    }
}

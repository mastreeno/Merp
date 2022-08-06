namespace Merp.Accountancy.Web.Core.Configuration
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
        string GetDraftsConnectionString();
    }
}

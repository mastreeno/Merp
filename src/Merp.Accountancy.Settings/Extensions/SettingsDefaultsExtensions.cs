using Merp.Accountancy.Settings.Model;
using System;
using System.Linq;

namespace Merp.Accountancy.Settings
{
    public static class SettingsDefaultsExtensions
    {
        public static IQueryable<SettingsDefaults> BySubscriptionId(this IQueryable<SettingsDefaults> settingsDefaults, Guid subscriptionId)
        {
            return settingsDefaults.Where(s => s.SubscriptionId == subscriptionId);
        }
    }
}

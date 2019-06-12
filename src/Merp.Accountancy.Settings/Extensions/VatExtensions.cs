using Merp.Accountancy.Settings.Model;
using System;
using System.Linq;

namespace Merp.Accountancy.Settings
{
    public static class VatExtensions
    {
        public static IQueryable<Vat> ByCountry(this IQueryable<Vat> vats, string country)
        {
            return vats.Where(v => v.Country == country);
        }

        public static IQueryable<Vat> NotUnlisted(this IQueryable<Vat> vats)
        {
            return vats.Where(v => !v.Unlisted);
        }

        public static IQueryable<Vat> BySubscriptionId(this IQueryable<Vat> vats, Guid subscriptionId)
        {
            return vats.Where(v => v.SubscriptionId == subscriptionId);
        }

        public static IQueryable<Vat> SystemOnly(this IQueryable<Vat> vats)
        {
            return vats.Where(v => v.SubscriptionId == Guid.Empty);
        }

        public static IQueryable<Vat> SystemAndBySubscriptionId(this IQueryable<Vat> vats, Guid subscriptionId)
        {
            return vats.Where(v => v.SubscriptionId == Guid.Empty || v.SubscriptionId == subscriptionId);
        }
    }
}

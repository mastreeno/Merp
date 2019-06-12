using Merp.Accountancy.Settings.Model;
using System.Linq;

namespace Merp.Accountancy.Settings
{
    public static class ProvidenceFundExtensions
    {
        public static IQueryable<ProvidenceFund> ByCountry(this IQueryable<ProvidenceFund> providenceFunds, string country)
        {
            return providenceFunds.Where(p => p.Country == country);
        }
    }
}

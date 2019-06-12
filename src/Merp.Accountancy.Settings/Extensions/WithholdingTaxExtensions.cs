using Merp.Accountancy.Settings.Model;
using System.Linq;

namespace Merp.Accountancy.Settings
{
    public static class WithholdingTaxExtensions
    {
        public static IQueryable<WithholdingTax> ByCountry(this IQueryable<WithholdingTax> withholdingTaxes, string country)
        {
            return withholdingTaxes.Where(w => w.Country == country);
        }
    }
}

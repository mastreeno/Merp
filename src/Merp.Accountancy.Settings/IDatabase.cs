using Merp.Accountancy.Settings.Model;
using System.Linq;

namespace Merp.Accountancy.Settings
{
    public interface IDatabase
    {
        IQueryable<Vat> Vats { get; }

        IQueryable<ProvidenceFund> ProvidenceFunds { get; }

        IQueryable<WithholdingTax> WithholdingTaxes { get; }

        IQueryable<SettingsDefaults> SettingsDefaults { get; }
    }
}

using System;
using System.Linq;
using Merp.Accountancy.Settings.Model;

namespace Merp.Accountancy.Settings
{
    public class Database : IDisposable, IDatabase
    {
        private readonly AccountancySettingsDbContext _context;

        public Database(AccountancySettingsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Vat> Vats => _context.Vats;

        public IQueryable<ProvidenceFund> ProvidenceFunds => _context.ProvidenceFunds;

        public IQueryable<WithholdingTax> WithholdingTaxes => _context.WithholdingTaxes;

        public IQueryable<SettingsDefaults> SettingsDefaults => _context.SettingsDefaults;

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}

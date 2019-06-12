using Merp.Accountancy.Settings.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Settings.Commands
{
    public class SettingsDefaultsCommands : IDisposable
    {
        private readonly AccountancySettingsDbContext _context;

        public SettingsDefaultsCommands(AccountancySettingsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public async Task SaveSettingsDefaults(Guid subscriptionId, bool minimumTaxPayerRegime, bool electronicInvoiceEnabled, bool splitPaymentApplied, Guid? vatId, Guid? providenceFundId, Guid? withholdingTaxId)
        {
            var defaults = _context.SettingsDefaults
                .BySubscriptionId(subscriptionId)
                .Include(s => s.Vat)
                .Include(s => s.WithholdingTax)
                .Include(s => s.ProvidenceFund)
                .SingleOrDefault();

            Vat vat = null;
            if (vatId.HasValue)
            {
                vat = _context.Vats.SingleOrDefault(v => v.Id == vatId.Value);
            }

            ProvidenceFund providenceFund = null;
            if (providenceFundId.HasValue)
            {
                providenceFund = _context.ProvidenceFunds.SingleOrDefault(p => p.Id == providenceFundId.Value);
            }

            WithholdingTax withholdingTax = null;
            if (withholdingTaxId.HasValue)
            {
                withholdingTax = _context.WithholdingTaxes.SingleOrDefault(w => w.Id == withholdingTaxId.Value);
            }

            if (defaults == null)
            {
                CreateSettingsDefaults(
                    subscriptionId, 
                    minimumTaxPayerRegime, 
                    electronicInvoiceEnabled, 
                    splitPaymentApplied, 
                    vat, 
                    providenceFund, 
                    withholdingTax);
            }
            else
            {
                EditSettingsDefaults(
                    defaults,
                    minimumTaxPayerRegime,
                    electronicInvoiceEnabled,
                    splitPaymentApplied,
                    vat,
                    providenceFund,
                    withholdingTax);
            }

            await _context.SaveChangesAsync();
        }

        #region Private methods
        private void CreateSettingsDefaults(Guid subscriptionId, bool minimumTaxPayerRegime, bool electronicInvoiceEnabled, bool splitPaymentApplied, Vat vat, ProvidenceFund providenceFund, WithholdingTax withholdingTax)
        {
            var defaults = new SettingsDefaults
            {
                Id = Guid.NewGuid(),
                SubscriptionId = subscriptionId,
                ElectronicInvoiceEnabled = electronicInvoiceEnabled,
                MinimumTaxPayerRegime = minimumTaxPayerRegime,
                SplitPaymentApplied = splitPaymentApplied,
                ProvidenceFund = providenceFund,
                Vat = vat,
                WithholdingTax = withholdingTax
            };

            _context.Add(defaults);
        }

        private void EditSettingsDefaults(SettingsDefaults defaults, bool minimumTaxPayerRegime, bool electronicInvoiceEnabled, bool splitPaymentApplied, Vat vat, ProvidenceFund providenceFund, WithholdingTax withholdingTax)
        {
            if (defaults.MinimumTaxPayerRegime != minimumTaxPayerRegime)
            {
                defaults.MinimumTaxPayerRegime = minimumTaxPayerRegime;
            }

            if (defaults.ElectronicInvoiceEnabled != electronicInvoiceEnabled)
            {
                defaults.ElectronicInvoiceEnabled = electronicInvoiceEnabled;
            }

            if (defaults.SplitPaymentApplied != splitPaymentApplied)
            {
                defaults.SplitPaymentApplied = splitPaymentApplied;
            }

            if (defaults.Vat?.Id != vat?.Id)
            {
                defaults.Vat = vat;
            }

            if (defaults.ProvidenceFund?.Id != providenceFund?.Id)
            {
                defaults.ProvidenceFund = providenceFund;
            }

            if (defaults.WithholdingTax?.Id != withholdingTax?.Id)
            {
                defaults.WithholdingTax = withholdingTax;
            }
        }
        #endregion
    }
}

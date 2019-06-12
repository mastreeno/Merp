using System;
using System.Linq;
using System.Threading.Tasks;
using Merp.Accountancy.Settings;
using Merp.Accountancy.Settings.Commands;
using Merp.Accountancy.Web.Api.Internal.Models.Settings;
using Microsoft.EntityFrameworkCore;

namespace Merp.Accountancy.Web.Api.Internal.WorkerServices
{
    public class SettingsControllerWorkerServices
    {
        public IDatabase Database { get; private set; }

        public SettingsDefaultsCommands Commands { get; private set; }

        public SettingsControllerWorkerServices(IDatabase database, SettingsDefaultsCommands commands)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public DefaultsModel GetSettingsDefaults()
        {
            var subscriptionId = Guid.Empty;
            var model = new DefaultsModel();

            var defaults = Database.SettingsDefaults
                .BySubscriptionId(subscriptionId)
                .Include(s => s.Vat)
                .Include(s => s.WithholdingTax)
                .Include(s => s.ProvidenceFund)
                .SingleOrDefault();

            if (defaults != null)
            {
                var vat = defaults.Vat;
                var providenceFund = defaults.ProvidenceFund;
                var withholdingTax = defaults.WithholdingTax;

                model.ElectronicInvoiceEnabled = defaults.ElectronicInvoiceEnabled;
                model.MinimumTaxpayerRegime = defaults.MinimumTaxPayerRegime;
                model.SplitPayment = defaults.SplitPaymentApplied;
                model.VatRate = vat == null ? null : new DefaultsModel.VatRateDescriptor
                {
                    Id = vat.Id,
                    Rate = vat.Rate
                };

                model.ProvidenceFund = providenceFund == null ? null : new DefaultsModel.ProvidenceFundDescriptor
                {
                    Id = providenceFund.Id,
                    Description = providenceFund.Description,
                    Rate = providenceFund.Rate,
                    AppliedInWithholdingTax = providenceFund.AppliedInWithholdingTax
                };

                model.WithholdingTax = withholdingTax == null ? null : new DefaultsModel.WithholdingTaxDescriptor
                {
                    Id = withholdingTax.Id,
                    Description = withholdingTax.Description,
                    Rate = withholdingTax.Rate,
                    TaxableAmountRate = withholdingTax.TaxableAmountRate
                };
            }

            return model;
        }

        public async Task SaveSettingsDefaults(DefaultsModel model)
        {
            var subscriptionId = Guid.Empty;

            await Commands.SaveSettingsDefaults(
                subscriptionId,
                model.MinimumTaxpayerRegime,
                model.ElectronicInvoiceEnabled,
                model.SplitPayment,
                model.VatRate?.Id,
                model.ProvidenceFund?.Id,
                model.WithholdingTax?.Id);
        }
    }
}

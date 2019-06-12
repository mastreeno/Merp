using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Merp.Accountancy.Settings.Model
{
    public class SettingsDefaults : IEntityTypeConfiguration<SettingsDefaults>
    {
        public Guid Id { get; set; }

        public Guid SubscriptionId { get; set; }

        public bool MinimumTaxPayerRegime { get; set; }

        public bool ElectronicInvoiceEnabled { get; set; }

        public bool SplitPaymentApplied { get; set; }

        public virtual Vat Vat { get; set; }

        public virtual WithholdingTax WithholdingTax { get; set; }

        public virtual ProvidenceFund ProvidenceFund { get; set; }

        public void Configure(EntityTypeBuilder<SettingsDefaults> builder)
        {
            builder.HasIndex(s => s.SubscriptionId);
            builder.HasOne(s => s.Vat);
            builder.HasOne(s => s.WithholdingTax);
            builder.HasOne(s => s.ProvidenceFund);
        }
    }
}

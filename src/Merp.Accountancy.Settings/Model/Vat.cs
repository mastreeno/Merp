using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Merp.Accountancy.Settings.Model
{
    public class Vat : IEntityTypeConfiguration<Vat>
    {
        public Guid Id { get; set; }

        public Guid SubscriptionId { get; set; }

        public string Country { get; set; }

        public decimal Rate { get; set; }

        public string Description { get; set; }

        public bool Unlisted { get; set; }

        public bool AppliedForMinimumTaxPayer { get; set; }

        public void Configure(EntityTypeBuilder<Vat> builder)
        {
            builder.HasIndex(v => v.SubscriptionId);
            builder.HasIndex(v => v.Rate);
            builder.HasIndex(v => v.Description);
        }
    }
}

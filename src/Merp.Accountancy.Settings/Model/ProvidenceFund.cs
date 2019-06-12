using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Merp.Accountancy.Settings.Model
{
    public class ProvidenceFund : IEntityTypeConfiguration<ProvidenceFund>
    {
        public Guid Id { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }

        public decimal Rate { get; set; }

        public bool AppliedInWithholdingTax { get; set; }

        public void Configure(EntityTypeBuilder<ProvidenceFund> builder)
        {
            builder.HasIndex(p => p.Rate);
            builder.HasIndex(p => p.Description);
        }
    }
}

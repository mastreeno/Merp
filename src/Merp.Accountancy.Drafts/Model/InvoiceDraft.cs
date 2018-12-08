using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Merp.Accountancy.Drafts.Model
{
    public class InvoiceDraft : IEntityTypeConfiguration<InvoiceDraft>
    {
        public Guid Id { get; set; }

        public DateTime? Date { get; set; }

        public string Currency { get; set; }

        public decimal TaxableAmount { get; set; }

        public decimal Taxes { get; set; }

        public decimal TotalPrice { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public string PaymentTerms { get; set; }

        public string Description { get; set; }

        public bool PricesAreVatIncluded { get; set; }

        public PartyInfo Customer { get; set; }

        public virtual List<DraftLineItem> LineItems { get; set; }

        public virtual List<PriceByVat> PricesByVat { get; set; }

        public virtual List<NonTaxableItem> NonTaxableItems { get; set; }

        public InvoiceDraft()
        {
            LineItems = new List<DraftLineItem>();
            PricesByVat = new List<PriceByVat>();
            NonTaxableItems = new List<NonTaxableItem>();
            Customer = new PartyInfo();
        }

        public void Configure(EntityTypeBuilder<InvoiceDraft> builder)
        {
            builder.HasIndex(o => o.Date);
            builder.HasIndex(o => o.PurchaseOrderNumber);

            builder.OwnsOne(c => c.Customer).HasIndex(o => o.Name);

            builder.HasMany(o => o.LineItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.PricesByVat).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.NonTaxableItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}

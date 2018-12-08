using Merp.Accountancy.Drafts.Model;
using Microsoft.EntityFrameworkCore;

namespace Merp.Accountancy.Drafts
{
    public class AccountancyDraftsDbContext : DbContext
    {
        public AccountancyDraftsDbContext() { }

        public AccountancyDraftsDbContext(DbContextOptions<AccountancyDraftsDbContext> options) : base(options) { }

        public DbSet<OutgoingInvoiceDraft> OutgoingInvoiceDrafts { get; set; }

        public DbSet<OutgoingCreditNoteDraft> OutgoingCreditNoteDrafts { get; set; }

        public DbSet<DraftLineItem> DraftLineItems { get; set; }

        public DbSet<PriceByVat> DraftPricesByVat { get; set; }

        public DbSet<NonTaxableItem> DraftNonTaxableItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InvoiceDraft());
        }
    }
}

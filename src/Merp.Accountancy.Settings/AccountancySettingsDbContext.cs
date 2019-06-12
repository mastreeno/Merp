using Merp.Accountancy.Settings.Model;
using Microsoft.EntityFrameworkCore;

namespace Merp.Accountancy.Settings
{
    public class AccountancySettingsDbContext : DbContext
    {
        public AccountancySettingsDbContext() { }

        public AccountancySettingsDbContext(DbContextOptions<AccountancySettingsDbContext> options)
            : base(options)
        {

        }

        public DbSet<Vat> Vats { get; set; }

        public DbSet<ProvidenceFund> ProvidenceFunds { get; set; }

        public DbSet<WithholdingTax> WithholdingTaxes { get; set; }

        public DbSet<SettingsDefaults> SettingsDefaults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Vat());
            modelBuilder.ApplyConfiguration(new ProvidenceFund());
            modelBuilder.ApplyConfiguration(new WithholdingTax());
            modelBuilder.ApplyConfiguration(new SettingsDefaults());
        }
    }
}

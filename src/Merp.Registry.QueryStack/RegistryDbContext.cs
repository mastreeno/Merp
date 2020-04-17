using Merp.Registry.QueryStack.Model;
using Microsoft.EntityFrameworkCore;

namespace Merp.Registry.QueryStack
{
    public class RegistryDbContext : DbContext
    {
        private readonly static string ContextName = "Registry";

        public DbSet<Party> Parties { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Company> Companies { get; set; }

        public RegistryDbContext() { }
        public RegistryDbContext(DbContextOptions<RegistryDbContext> options) : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Party
            var partyEntityBuilder = modelBuilder.Entity<Party>();
            //partyEntityBuilder.ToTable("Parties");

            partyEntityBuilder
                    .HasDiscriminator<Party.PartyType>("Type")
                    .HasValue<Party>(Party.PartyType.Unspecified)
                    .HasValue<Person>(Party.PartyType.Person)
                    .HasValue<Company>(Party.PartyType.Company);

            partyEntityBuilder.HasIndex(o => o.OriginalId);
            partyEntityBuilder.HasIndex(o => o.DisplayName);

            //partyEntityBuilder.OwnsOne(o => o.ContactInfo);
            partyEntityBuilder.OwnsOne(o => o.LegalAddress);
            partyEntityBuilder.OwnsOne(o => o.BillingAddress);
            partyEntityBuilder.OwnsOne(o => o.ShippingAddress);

            //Company
            //var companyEntityBuilder = modelBuilder.Entity<Company>();

            //Person


            //foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    entity
            //        .Relational()
            //        .TableName = $"{ContextName}_{entity.ClrType.Name}";
            //}
        }
    }
}

using Merp.Sales.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Merp.Sales.QueryStack
{
    public class SalesDbContext : DbContext
    {
        private readonly static string ContextName = "Sales";

        public SalesDbContext() { }
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options){ }

        public DbSet<BusinessProposal> Proposals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BusinessProposal());

            //foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    entity
            //        .Relational()
            //        .TableName = $"{ContextName}_{entity.ClrType.Name}";
            //}
        }
    }
}

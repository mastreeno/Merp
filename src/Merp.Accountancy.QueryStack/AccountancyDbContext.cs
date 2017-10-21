using Merp.Accountancy.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Merp.Accountancy.QueryStack
{
    public class AccountancyDbContext : DbContext
    {
        private readonly static string ContextName = "Accountancy";

        public AccountancyDbContext() { }
        public AccountancyDbContext(DbContextOptions<AccountancyDbContext> options) : base(options){ }

        public DbSet<JobOrder> JobOrders { get; set; }
        public DbSet<IncomingInvoice> IncomingInvoices { get; set; }
        public DbSet<OutgoingInvoice> OutgoingInvoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Invoice());
            modelBuilder.ApplyConfiguration(new JobOrder());

            //foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    entity
            //        .Relational()
            //        .TableName = $"{ContextName}_{entity.ClrType.Name}";
            //}
        }
    }
}

using Merp.Accountancy.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack
{
    public class AccountancyContext : DbContext
    {
        public AccountancyContext()
            : base("Merp")
        {

        }

        public AccountancyContext(string connectionString)
            : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Types().Configure(entity => entity.ToTable(string.Format("{0}_{1}", "Accountancy", entity.ClrType.Name)));
        }

        public DbSet<JobOrder> JobOrders { get; set; }

        public DbSet<IncomingInvoice> IncomingInvoices { get; set; }
        public DbSet<OutgoingInvoice> OutgoingInvoices { get; set; }
    }
}

using Merp.Accountancy.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack
{
    public class Database : IDatabase, IDisposable
    {
        private AccountancyContext Context;

        public Database()
        {
            Context = new AccountancyContext();
            Context.Configuration.AutoDetectChangesEnabled = false;
        }

        public Database(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            Context = new AccountancyContext(connectionString);
            Context.Configuration.AutoDetectChangesEnabled = false;
            Context.Configuration.LazyLoadingEnabled = false;
            Context.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<JobOrder> JobOrders
        {
            get
            {
                return Context.JobOrders;
            }
        }
        public IQueryable<IncomingInvoice> IncomingInvoices
        {
            get
            {
                return Context.IncomingInvoices;
            }
        }
        public IQueryable<OutgoingInvoice> OutgoingInvoices
        {
            get
            {
                return Context.OutgoingInvoices;
            }
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}

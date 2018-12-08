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
        private AccountancyDbContext Context;

        public Database(AccountancyDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IQueryable<JobOrder> JobOrders
        {
            get
            {
                return Context.JobOrders;
            }
        }
        public IQueryable<IncomingCreditNote> IncomingCreditNotes
        {
            get
            {
                return Context.IncomingCreditNotes;
            }
        }
        public IQueryable<IncomingInvoice> IncomingInvoices
        {
            get
            {
                return Context.IncomingInvoices;
            }
        }
        public IQueryable<OutgoingCreditNote> OutgoingCreditNotes
        {
            get
            {
                return Context.OutgoingCreditNotes;
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

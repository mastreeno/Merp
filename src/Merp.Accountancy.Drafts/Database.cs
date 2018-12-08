using System;
using System.Linq;
using Merp.Accountancy.Drafts.Model;

namespace Merp.Accountancy.Drafts
{
    public class Database : IDisposable, IDatabase
    {
        private readonly AccountancyDraftsDbContext _context;

        public Database(AccountancyDraftsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<OutgoingInvoiceDraft> OutgoingInvoiceDrafts => _context.OutgoingInvoiceDrafts;

        public IQueryable<OutgoingCreditNoteDraft> OutgoingCreditNoteDrafts => _context.OutgoingCreditNoteDrafts;

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}

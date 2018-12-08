using Merp.Accountancy.Drafts.Model;
using System.Linq;

namespace Merp.Accountancy.Drafts
{
    public interface IDatabase
    {
        IQueryable<OutgoingInvoiceDraft> OutgoingInvoiceDrafts { get; }

        IQueryable<OutgoingCreditNoteDraft> OutgoingCreditNoteDrafts { get; }
    }
}

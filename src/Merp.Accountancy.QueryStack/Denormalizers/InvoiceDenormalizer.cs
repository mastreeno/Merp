using Merp.Accountancy.CommandStack.Events;
using System.Linq;
using Rebus.Handlers;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Denormalizers
{
    public class InvoiceDenormalizer :
        IHandleMessages<IncomingInvoiceLinkedToJobOrderEvent>,
        IHandleMessages<OutgoingInvoiceLinkedToJobOrderEvent>
    {
        public async Task Handle(IncomingInvoiceLinkedToJobOrderEvent message)
        {
            using (var ctx = new AccountancyContext())
            {
                var invoice = ctx.IncomingInvoices.Where(i => i.OriginalId == message.InvoiceId).Single();
                invoice.JobOrderId = message.JobOrderId;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(OutgoingInvoiceLinkedToJobOrderEvent message)
        {
            using (var ctx = new AccountancyContext())
            {
                var invoice = ctx.OutgoingInvoices.Where(i => i.OriginalId == message.InvoiceId).Single();
                invoice.JobOrderId = message.JobOrderId;
                await ctx.SaveChangesAsync();
            }
        }
    }
}

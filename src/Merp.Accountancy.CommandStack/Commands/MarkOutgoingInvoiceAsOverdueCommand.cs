using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkOutgoingInvoiceAsOverdueCommand : MerpCommand
    {
        public Guid InvoiceId { get; set; }

        public MarkOutgoingInvoiceAsOverdueCommand(Guid userId, Guid invoiceId)
            : base(userId)
        {
            InvoiceId = invoiceId;
        }
    }
}

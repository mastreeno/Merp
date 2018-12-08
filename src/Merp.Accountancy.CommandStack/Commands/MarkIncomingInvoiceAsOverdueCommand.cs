using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkIncomingInvoiceAsOverdueCommand : MerpCommand
    {
        public Guid InvoiceId { get; set; }

        public MarkIncomingInvoiceAsOverdueCommand(Guid userId, Guid invoiceId)
            : base(userId)
        {
            InvoiceId = invoiceId;
        }
    }
}

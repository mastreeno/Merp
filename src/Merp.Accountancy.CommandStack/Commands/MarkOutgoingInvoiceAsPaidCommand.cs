using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkOutgoingInvoiceAsPaidCommand : MerpCommand
    {
        public Guid InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }

        public MarkOutgoingInvoiceAsPaidCommand(Guid userId, Guid invoiceId, DateTime paymentDate)
            : base(userId)
        {
            InvoiceId = invoiceId;
            PaymentDate = paymentDate;
        }
    }
}

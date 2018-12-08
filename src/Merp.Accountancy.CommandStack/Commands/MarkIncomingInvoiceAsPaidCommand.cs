using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkIncomingInvoiceAsPaidCommand : MerpCommand
    {
        public Guid InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }

        public MarkIncomingInvoiceAsPaidCommand(Guid userId, Guid invoiceId, DateTime paymentDate)
            : base(userId)
        {
            InvoiceId = invoiceId;
            PaymentDate = paymentDate;
        }
    }
}

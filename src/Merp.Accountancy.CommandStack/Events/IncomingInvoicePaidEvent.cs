using MementoFX.Domain;
using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingInvoicePaidEvent : MerpDomainEvent
    {
        public Guid InvoiceId { get; set; }
        [Timestamp]
        public DateTime PaymentDate { get; set; }

        public IncomingInvoicePaidEvent(Guid invoiceId, DateTime paymentDate, Guid userId)
            : base(userId)
        {
            InvoiceId = invoiceId;
            PaymentDate = paymentDate;
        }
    }
}

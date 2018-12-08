using System;
using MementoFX.Domain;
using Merp.Domain;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingInvoiceLinkedToJobOrderEvent : MerpDomainEvent
    {
        public Guid InvoiceId { get; set; }

        public Guid JobOrderId { get; set; }

        [Timestamp]
        public DateTime DateOfLink { get; set; }

        public decimal Amount { get; set; }

        public IncomingInvoiceLinkedToJobOrderEvent(Guid invoiceId, Guid jobOrderId, DateTime DateOfLink, decimal amount, Guid userId)
            : base(userId)
        {
            InvoiceId = invoiceId;
            JobOrderId = jobOrderId;
            this.DateOfLink = DateOfLink;
            Amount = amount;
        }
    }
}

using System;
using MementoFX.Domain;
using Merp.Domain;

namespace Merp.Accountancy.CommandStack.Events
{
    public class OutgoingInvoiceLinkedToJobOrderEvent : MerpDomainEvent
    {
        public Guid InvoiceId { get; set; }

        public Guid JobOrderId { get; set; }

        [Timestamp]
        public DateTime DateOfLink { get; set; }

        public decimal Amount { get; set; }

        public OutgoingInvoiceLinkedToJobOrderEvent(Guid invoiceId, Guid jobOrderId, DateTime dateOfLink, decimal amount, Guid userId)
            : base(userId)
        {
            InvoiceId = invoiceId;
            JobOrderId = jobOrderId;
            DateOfLink = dateOfLink;
            Amount = amount;
        }
    }
}

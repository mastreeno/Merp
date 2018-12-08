using MementoFX.Domain;
using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Events
{
    public class OutgoingInvoiceGotOverdueEvent : MerpDomainEvent
    {
        public Guid InvoiceId { get; set; }

        [Timestamp]
        public DateTime DueDate { get; set; }

        public OutgoingInvoiceGotOverdueEvent(Guid invoiceId, DateTime dueDate, Guid userId)
            : base(userId)
        {
            InvoiceId = invoiceId;
            DueDate = dueDate;
        }
    }
}

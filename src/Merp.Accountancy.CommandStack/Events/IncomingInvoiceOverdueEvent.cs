using MementoFX.Domain;
using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingInvoiceOverdueEvent : MerpDomainEvent
    {
        public Guid InvoiceId { get; set; }

        [Timestamp]
        public DateTime DueDate { get; set; }

        public IncomingInvoiceOverdueEvent(Guid invoiceId, DateTime dueDate, Guid userId)
            : base(userId)
        {
            InvoiceId = invoiceId;
            DueDate = dueDate;
        }
    }
}

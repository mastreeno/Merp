using MementoFX.Domain;
using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingInvoiceGotOverdueEvent : MerpDomainEvent
    {
        public Guid InvoiceId { get; set; }

        [Timestamp]
        public DateTime DueDate { get; set; }

        public IncomingInvoiceGotOverdueEvent(Guid invoiceId, DateTime dueDate, Guid userId)
            : base(userId)
        {
            InvoiceId = invoiceId;
            DueDate = dueDate;
        }
    }
}

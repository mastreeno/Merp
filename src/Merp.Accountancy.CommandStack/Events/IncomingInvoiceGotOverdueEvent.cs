using MementoFX;
using MementoFX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingInvoiceGotOverdueEvent : DomainEvent
    {
        public Guid InvoiceId { get; set; }

        [Timestamp]
        public DateTime DueDate { get; set; }

        public IncomingInvoiceGotOverdueEvent(Guid invoiceId, DateTime dueDate)
        {
            InvoiceId = invoiceId;
            DueDate = dueDate;
        }
    }
}

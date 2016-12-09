using Memento;
using Memento.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Events
{
    public class OutgoingInvoiceExpiredEvent : DomainEvent
    {
        public Guid InvoiceId { get; set; }
        [Timestamp]
        public DateTime DueDate { get; set; }

        public OutgoingInvoiceExpiredEvent(Guid invoiceId, DateTime expirationDate)
        {
            InvoiceId = invoiceId;
            DueDate = expirationDate;
        }
    }
}

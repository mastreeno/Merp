using Memento;
using Memento.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingInvoiceExpiredEvent : DomainEvent
    {
        public Guid InvoiceId { get; set; }
        [Timestamp]
        public DateTime DueDate { get; set; }

        public IncomingInvoiceExpiredEvent(Guid invoiceId, DateTime expirationDate)
        {
            InvoiceId = invoiceId;
            DueDate = expirationDate;
        }
    }
}

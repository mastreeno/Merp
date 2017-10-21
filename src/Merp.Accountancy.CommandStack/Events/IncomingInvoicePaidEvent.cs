using MementoFX;
using MementoFX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingInvoicePaidEvent : DomainEvent
    {
        public Guid InvoiceId { get; set; }
        [Timestamp]
        public DateTime PaymentDate { get; set; }

        public IncomingInvoicePaidEvent(Guid invoiceId, DateTime paymentDate)
        {
            InvoiceId = invoiceId;
            PaymentDate = paymentDate;
        }
    }
}

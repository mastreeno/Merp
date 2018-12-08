using MementoFX;
using MementoFX.Domain;
using Merp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Events
{
    public class OutgoingInvoicePaidEvent : MerpDomainEvent
    {
        public Guid InvoiceId { get; set; }
        [Timestamp]
        public DateTime PaymentDate { get; set; }

        public OutgoingInvoicePaidEvent(Guid invoiceId, DateTime paymentDate, Guid userId)
            : base(userId)
        {
            InvoiceId = invoiceId;
            PaymentDate = paymentDate;
        }
    }
}

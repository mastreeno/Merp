using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using MementoFX.Domain;

namespace Merp.Accountancy.CommandStack.Events
{
    public class OutgoingInvoiceLinkedToJobOrderEvent : DomainEvent
    {
        public Guid InvoiceId { get; set; }

        public Guid JobOrderId { get; set; }

        [Timestamp]
        public DateTime DateOfLink { get; set; }

        public decimal Amount { get; set; }

        public OutgoingInvoiceLinkedToJobOrderEvent(Guid invoiceId, Guid jobOrderId, DateTime dateOfLink, decimal amount)
        {
            InvoiceId = invoiceId;
            JobOrderId = jobOrderId;
            DateOfLink = dateOfLink;
            Amount = amount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class LinkOutgoingInvoiceToJobOrderCommand : Command
    {
        public Guid JobOrderId { get; set; }

        public Guid InvoiceId { get; set; }

        [Timestamp]
        public DateTime DateOfLink { get; set; }

        public decimal Amount { get; set; }

        public LinkOutgoingInvoiceToJobOrderCommand(Guid invoiceId, Guid jobOrderId, DateTime dateOfLink, decimal amount)
        {
            InvoiceId = invoiceId;
            JobOrderId = jobOrderId;
            DateOfLink = dateOfLink;
            Amount = amount;
        }
    }
}

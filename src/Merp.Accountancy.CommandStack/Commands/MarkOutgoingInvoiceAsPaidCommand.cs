using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkOutgoingInvoiceAsPaidCommand : Command
    {
        public Guid InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }

        public MarkOutgoingInvoiceAsPaidCommand(Guid invoiceId, DateTime paymentDate)
        {
            InvoiceId = invoiceId;
            PaymentDate = paymentDate;
        }
    }
}

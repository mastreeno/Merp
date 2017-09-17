using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkOutgoingInvoiceAsExpiredCommand : Command
    {
        public Guid InvoiceId { get; set; }

        public MarkOutgoingInvoiceAsExpiredCommand(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}

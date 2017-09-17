using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkIncomingInvoiceAsExpiredCommand : Command
    {
        public Guid InvoiceId { get; set; }

        public MarkIncomingInvoiceAsExpiredCommand(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}

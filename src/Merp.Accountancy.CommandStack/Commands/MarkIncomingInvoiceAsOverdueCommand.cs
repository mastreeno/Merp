using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkIncomingInvoiceAsOverdueCommand : Command
    {
        public Guid InvoiceId { get; set; }

        public MarkIncomingInvoiceAsOverdueCommand(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}

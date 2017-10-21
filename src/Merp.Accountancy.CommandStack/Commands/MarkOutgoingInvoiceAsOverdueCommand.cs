using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkOutgoingInvoiceAsOverdueCommand : Command
    {
        public Guid InvoiceId { get; set; }

        public MarkOutgoingInvoiceAsOverdueCommand(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}

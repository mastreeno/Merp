using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Model
{
    public class IncomingInvoice : Invoice
    {
        public PartyInfo Supplier { get; set; }
    }
}

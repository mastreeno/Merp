using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Services
{
    public class OutgoingInvoiceNumberGenerator : IOutgoingInvoiceNumberGenerator
    {
        public string Generate()
        {
            return string.Format("{0}/{1:yyyy}", Math.Abs(Guid.NewGuid().GetHashCode()), DateTime.Now);
        }
    }
}

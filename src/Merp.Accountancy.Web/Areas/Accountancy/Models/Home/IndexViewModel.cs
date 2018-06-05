using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Accountancy.Models.Home
{
    public class IndexViewModel
    {
        public decimal OutstandingOutgoingInvoicesCount { get; set; }
        public decimal OverdueOutgoingInvoicesCount { get; set; }
        public decimal OutstandingOutgoingInvoicesTotalPrice { get; set; }
        public decimal OverdueOutgoingInvoicesTotalPrice { get; set; }

        public decimal OutstandingIncomingInvoicesCount { get; set; }
        public decimal OverdueIncomingInvoicesCount { get; set; }
        public decimal OutstandingIncomingInvoicesTotalPrice { get; set; }
        public decimal OverdueIncomingInvoicesTotalPrice { get; set; }
    }
}

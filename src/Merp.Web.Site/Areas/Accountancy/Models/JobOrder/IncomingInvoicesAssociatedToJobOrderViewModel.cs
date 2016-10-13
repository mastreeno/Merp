using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class IncomingInvoicesAssociatedToJobOrderViewModel
    {
        public IEnumerable<Invoice> IncomingInvoices { get; set; }

        public class Invoice
        {
            public string Number { get; set; }
            public DateTime DateOfIssue { get; set; }
            public decimal Price { get; set; }
            public string SupplierName { get; set; }
        }
    }
}
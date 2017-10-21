using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Accountancy.Models.Invoice
{
    public class Search_GetInvoiceListViewModel
    {
        public IEnumerable<Invoice> Invoices { get; set; }

        public class Invoice
        {
            public Guid Uid { get; set; }
            public string DocumentType { get; set; }
            public string Number { get; set; }
            public DateTime Date { get; set; }
            public DateTime? DueDate { get; set; }
            public string SupplierName { get; set; }
            public string CustomerName { get; set; }
            public decimal TotalPrice { get; set; }
            public string Currency { get; set; }
        }
    }
}

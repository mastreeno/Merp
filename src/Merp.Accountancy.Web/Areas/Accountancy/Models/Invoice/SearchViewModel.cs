using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Merp.Web.Site.Areas.Registry.Models;

namespace Merp.Web.Site.Areas.Accountancy.Models.Invoice
{
    public class SearchViewModel
    {
        [DisplayName("Date from:")]
        public DateTime DateFrom { get; set; }

        [DisplayName("Date to")]
        public DateTime DateTo { get; set; }

        public PartyInfo Customer { get; set; }
        public PartyInfo Supplier { get; set; }

        public enum InvoiceKind
        {
            Any,
            Incoming,
            Outgoing
        }

        public enum InvoiceState
        {
            Outstanding,
            Overdue,
            Paid
        }
    }
}

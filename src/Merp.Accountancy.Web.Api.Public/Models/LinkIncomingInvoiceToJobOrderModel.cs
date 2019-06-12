using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public.Models
{
    public class LinkIncomingInvoiceToJobOrderModel
    {
        public decimal Amount { get; set; }

        public DateTime DateOfLink { get; set; }

        public Guid InvoiceId { get; set; }

        public Guid JobOrderId { get; set; }

        public Guid UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class FixedPriceJobOrderDetailViewModel
    {
        public Guid JobOrderId { get; set; }
        public string JobOrderNumber { get; set; }
        public string JobOrderName { get; set; }
        public string CustomerName { get; set; }
        public decimal Price { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DueDate { get; set; }
        public string Notes { get; set; }
        public bool IsCompleted { get; set; }
        public decimal Balance { get; set; }
    }
}
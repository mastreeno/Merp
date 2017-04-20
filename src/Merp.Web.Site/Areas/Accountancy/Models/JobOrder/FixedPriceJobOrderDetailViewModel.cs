using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class FixedPriceJobOrderDetailViewModel
    {
        [DisplayName("Job Order Id")]
        public Guid JobOrderId { get; set; }

        [DisplayName("Job Order #")]
        public string JobOrderNumber { get; set; }

        [DisplayName("Job Order name")]
        public string JobOrderName { get; set; }

        [DisplayName("Customer name")]
        public string CustomerName { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Date of start")]
        public DateTime DateOfStart { get; set; }

        [DisplayName("Due date")]
        public DateTime DueDate { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }

        public bool IsCompleted { get; set; }

        [DisplayName("Balance")]
        public decimal Balance { get; set; }
    }
}
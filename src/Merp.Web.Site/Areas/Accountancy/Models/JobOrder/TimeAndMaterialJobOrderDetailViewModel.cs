using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class TimeAndMaterialJobOrderDetailViewModel
    {
        public Guid JobOrderId { get; set; }

        [DisplayName("Job Order #")]
        public string JobOrderNumber { get; set; }

        [DisplayName("Job Order name")]
        public string JobOrderName { get; set; }

        [DisplayName("Customer name")]
        public string CustomerName { get; set; }

        [DisplayName("Value")]
        public decimal Value { get; set; }

        [DisplayName("Date of start")]
        public DateTime DateOfStart { get; set; }

        [DisplayName("Date of expiration")]
        public DateTime? DateOfExpiration { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }

        public bool IsCompleted { get; set; }

        [DisplayName("Balance")]
        public decimal Balance { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class JobOrderDetailViewModel
    {
        [DisplayName("Job Order Id")]
        public Guid JobOrderId { get; set; }

        [DisplayName("Job Order #")]
        public string JobOrderNumber { get; set; }

        [DisplayName("Job Order name")]
        public string JobOrderName { get; set; }

        [DisplayName("Customer")]
        public Guid CustomerId { get; set; }

        [DisplayName("Contact")]
        public Guid? ContactPersonId { get; set; }

        [DisplayName("Manager")]
        public Guid ManagerId { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Date of start")]
        public DateTime DateOfStart { get; set; }

        [DisplayName("Due date")]
        public DateTime DueDate { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        [DisplayName("Balance")]
        public decimal Balance { get; set; }
    }
}
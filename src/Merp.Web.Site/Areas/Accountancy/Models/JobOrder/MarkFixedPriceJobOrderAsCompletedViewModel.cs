using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class MarkFixedPriceJobOrderAsCompletedViewModel
    {
        [Required]
        public Guid JobOrderId { get; set; }
        public string JobOrderNumber { get; set; }
        public string JobOrderName { get; set; }
        public string CustomerName { get; set; }
        [Required]
        public DateTime DateOfCompletion { get; set; }
    }
}
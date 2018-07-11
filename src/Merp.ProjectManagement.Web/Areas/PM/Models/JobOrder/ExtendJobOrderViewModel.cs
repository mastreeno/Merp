using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class ExtendJobOrderViewModel : IValidatableObject
    {
        [Required]
        public Guid JobOrderId { get; set; }

        [DisplayName("Job Order #")]
        public string JobOrderNumber { get; set; }

        [DisplayName("Job Order name")]
        public string JobOrderName { get; set; }

        [DisplayName("Customer name")]
        public string CustomerName { get; set; }

        [DisplayName("Updated due date")]
        [Required]
        public DateTime NewDueDate { get; set; }

        [DisplayName("Updated price")]
        [Required]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Price <= 0)
            {
                var result = new ValidationResult("Price must be higher than zero.", new string[] { "Price" });
                results.Add(result);
            }
            return results;
        }
    }
}
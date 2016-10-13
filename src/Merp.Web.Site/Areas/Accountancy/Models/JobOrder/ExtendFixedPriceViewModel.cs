using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class ExtendFixedPriceViewModel : IValidatableObject
    {
        [Required]
        public Guid JobOrderId { get; set; }
        public string JobOrderNumber { get; set; }
        public string JobOrderName { get; set; }
        public string CustomerName { get; set; }
        [Required]
        public DateTime NewDueDate { get; set; }
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
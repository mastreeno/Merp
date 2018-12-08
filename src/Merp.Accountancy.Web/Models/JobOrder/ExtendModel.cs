using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models.JobOrder
{
    public class ExtendModel : IValidatableObject
    {
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

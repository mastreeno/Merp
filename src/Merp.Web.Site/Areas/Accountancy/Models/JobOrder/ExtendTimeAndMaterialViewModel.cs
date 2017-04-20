using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class ExtendTimeAndMaterialViewModel : IValidatableObject
    {
        [Required]
        public Guid JobOrderId { get; set; }

        [DisplayName("Job Order #")]
        public string JobOrderNumber { get; set; }

        [DisplayName("Job Order name")]
        public string JobOrderName { get; set; }

        [DisplayName("Customer name")]
        public string CustomerName { get; set; }

        [DisplayName("Updated date of expiration")]
        [Required]
        public DateTime? NewDateOfExpiration { get; set; }

        [DisplayName("Updated value")]
        [Required]
        public decimal Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            //if (!NewDateOfExpiration.HasValue && !Value.HasValue)
            //{
            //    var result = new ValidationResult("Either the new date of expiration or the value must be set.", new string[] { "NewDateOfExpiration", "Value" });
            //    results.Add(result);
            //}
            //if(Value.HasValue && Value.Value <= 0)
            //{
            //    var result = new ValidationResult("If specified, the value must be higher than zero.", new string[] { "Value" });
            //    results.Add(result);
            //}
            return results;
        }
    }
}
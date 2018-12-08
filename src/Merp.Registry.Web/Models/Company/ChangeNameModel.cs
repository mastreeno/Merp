using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Models.Company
{
    public class ChangeNameModel : IValidatableObject
    {
        [Required]
        [DisplayName("New Company Name")]
        public string NewCompanyName { get; set; }

        [Required]
        [DisplayName("Effective Date")]
        public DateTime EffectiveDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (EffectiveDate > DateTime.Now)
            {
                validationResults.Add(new ValidationResult($"The Company Name change cannot be scheduled in the future", new[] { $"{nameof(EffectiveDate)}" }));
            }

            return validationResults;
        }
    }
}

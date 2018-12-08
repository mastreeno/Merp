using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Models.Company
{
    public class ChangeLegalAddressModel : IValidatableObject
    {
        [DisplayName("Legal Address")]
        public PostalAddress LegalAddress { get; set; }

        [Required]
        [DisplayName("Effective Date")]
        public DateTime EffectiveDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(LegalAddress.Address))
            {
                validationResults.Add(new ValidationResult($"{nameof(LegalAddress.Address)} is required", new[] { $"{nameof(LegalAddress)}.{nameof(LegalAddress.Address)}" }));
            }
            if (string.IsNullOrWhiteSpace(LegalAddress.City))
            {
                validationResults.Add(new ValidationResult($"{nameof(LegalAddress.City)} is required", new[] { $"{nameof(LegalAddress)}.{nameof(LegalAddress.City)}" }));
            }

            if (EffectiveDate > DateTime.Now)
            {
                validationResults.Add(new ValidationResult($"The {nameof(LegalAddress)} change cannot be scheduled in the future", new[] { $"{nameof(EffectiveDate)}" }));
            }

            return validationResults;
        }
    }
}

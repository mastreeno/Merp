using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Models.Company
{
    public class ChangeBillingAddressModel : IValidatableObject
    {
        [DisplayName("Billing Address")]
        public PostalAddress BillingAddress { get; set; }

        public DateTime EffectiveDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(BillingAddress.Address))
            {
                validationResults.Add(new ValidationResult($"{nameof(BillingAddress.Address)} is required", new[] { $"{nameof(BillingAddress)}.{nameof(BillingAddress.Address)}" }));
            }
            if (string.IsNullOrWhiteSpace(BillingAddress.City))
            {
                validationResults.Add(new ValidationResult($"{nameof(BillingAddress.City)} is required", new[] { $"{nameof(BillingAddress)}.{nameof(BillingAddress.City)}" }));
            }

            return validationResults;
        }
    }
}

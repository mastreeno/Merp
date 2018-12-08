using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Models.Company
{
    public class ChangeShippingAddressModel : IValidatableObject
    {
        [DisplayName("Shipping Address")]
        public PostalAddress ShippingAddress { get; set; }

        public DateTime EffectiveDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(ShippingAddress.Address))
            {
                validationResults.Add(new ValidationResult($"{nameof(ShippingAddress.Address)} is required", new[] { $"{nameof(ShippingAddress)}.{nameof(ShippingAddress.Address)}" }));
            }

            if (string.IsNullOrWhiteSpace(ShippingAddress.City))
            {
                validationResults.Add(new ValidationResult($"{nameof(ShippingAddress.City)} is required", new[] { $"{nameof(ShippingAddress)}.{nameof(ShippingAddress.City)}" }));
            }

            return validationResults;
        }
    }
}

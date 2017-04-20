using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class AddEntryViewModel : IValidatableObject
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string VatNumber { get; set; }
        public string NationalIdentificationNumber { get; set; }
        public PostalAddress LegalAddress { get; set; }
        public PostalAddress BillingAddress { get; set; }
        public PostalAddress ShippingAddress { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(LegalAddress.Address) || string.IsNullOrWhiteSpace(LegalAddress.City))
            {
                validationResults.Add(new ValidationResult($"{nameof(LegalAddress)} is required"));
            }

            if (!BillingAddress.IsValid)
            {
                validationResults.Add(new ValidationResult($"Invalid {nameof(BillingAddress)}: {nameof(BillingAddress.Address)} and {nameof(BillingAddress.City)} are mandatory when setting an address", new[] { nameof(BillingAddress) }));
            }

            if (!ShippingAddress.IsValid)
            {
                validationResults.Add(new ValidationResult($"Invalid {nameof(ShippingAddress)}: {nameof(ShippingAddress.Address)} and {nameof(ShippingAddress.City)} are mandatory when setting an address", new[] { nameof(ShippingAddress) }));
            }

            return validationResults;
        }
    }
}
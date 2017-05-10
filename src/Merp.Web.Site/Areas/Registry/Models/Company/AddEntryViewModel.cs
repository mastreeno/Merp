using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class AddEntryViewModel : IValidatableObject
    {
        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [DisplayName("Vat Number")]
        public string VatNumber { get; set; }
        [DisplayName("National Identification Number")]
        public string NationalIdentificationNumber { get; set; }
        [DisplayName("Legal Address")]
        public PostalAddress LegalAddress { get; set; }
        [DisplayName("Billing Address")]
        public PostalAddress BillingAddress { get; set; }
        [DisplayName("Shipping Address")]
        public PostalAddress ShippingAddress { get; set; }
        [DisplayName("Main Contact")]
        public PersonInfo MainContact { get; set; }
        [DisplayName("Administrative Contact")]
        public PersonInfo AdministrativeContact { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Fax Number")]
        public string FaxNumber { get; set; }
        [DisplayName("Website Address")]
        public string WebsiteAddress { get; set; }
        [DisplayName("Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

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
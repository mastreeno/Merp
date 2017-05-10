using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Person
{
    public class AddEntryViewModel : IValidatableObject
    {
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        [DisplayName("National Identification Number")]
        [Required]
        public string NationalIdentificationNumber { get; set; }
        [DisplayName("VAT number")]
        public string VatNumber { get; set; }
        [DisplayName("Address")]
        public PostalAddress Address { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        [DisplayName("Fax Number")]
        public string FaxNumber { get; set; }
        [DisplayName("Website Address")]
        public string WebsiteAddress { get; set; }
        [DisplayName("Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [DisplayName("IM")]
        public string InstantMessaging { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (!Address.IsValid)
            {
                validationResults.Add(new ValidationResult($"Invalid {nameof(Address)}: {nameof(Address.Address)} and {nameof(Address.City)} are mandatory when setting an address"));
            }

            return validationResults;
        }
    }
}
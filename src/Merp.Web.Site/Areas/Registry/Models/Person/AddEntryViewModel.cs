using Merp.Registry.CommandStack.Helpers;
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
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        
        [StringLength(16, MinimumLength = 16, ErrorMessage = "National Identification Number must have a length of 16 characters")]
        [DisplayName("National Identification Number")]
        public string NationalIdentificationNumber { get; set; }

        [DisplayName("VAT number")]
        public string VatNumber { get; set; }
        
        [DisplayName("Address")]
        public PostalAddress Address { get; set; }
        
        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Phone]
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }

        [DisplayName("Fax Number")]
        public string FaxNumber { get; set; }
        
        [DisplayName("Website Address")]
        public string WebsiteAddress { get; set; }

        [EmailAddress]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("IM")]
        public string InstantMessaging { get; set; }

        [StringLength(40, MinimumLength = 5)]
        [DisplayName("Skype username")]
        public string Skype { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (!string.IsNullOrWhiteSpace(NationalIdentificationNumber))
            {
                if (!NationalIdentificationNumberHelper.IsMatchingFirstName(NationalIdentificationNumber.Trim().ToUpper(), FirstName))
                {
                    validationResults.Add(new ValidationResult("National Identification Number is not matching with First Name", new string[] { nameof(NationalIdentificationNumber) }));
                }
                if (!NationalIdentificationNumberHelper.IsMatchingLastName(NationalIdentificationNumber.Trim().ToUpper(), LastName))
                {
                    validationResults.Add(new ValidationResult("National Identification Number is not matching with Last Name", new string[] { nameof(NationalIdentificationNumber) }));
                }
            }

            return validationResults;
        }
    }
}
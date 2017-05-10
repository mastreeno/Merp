using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class ChangeLegalAddressViewModel : IValidatableObject
    {
        [Required]
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
   
        [DisplayName("Legal Address")]
        public PostalAddress LegalAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (!LegalAddress.IsValid)
            {
                validationResults.Add(new ValidationResult($"Invalid {nameof(LegalAddress)}: {nameof(LegalAddress.Address)} and {nameof(LegalAddress.City)} are mandatory when setting an address", new[] { nameof(LegalAddress) }));
            }

            return validationResults;
        }
    }
}
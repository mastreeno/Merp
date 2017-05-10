using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class ChangeBillingAddressViewModel : IValidatableObject
    {
        [Required]
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
   
        [DisplayName("Billing Address")]
        public PostalAddress BillingAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (!BillingAddress.IsValid)
            {
                validationResults.Add(new ValidationResult($"Invalid {nameof(BillingAddress)}: {nameof(BillingAddress.Address)} and {nameof(BillingAddress.City)} are mandatory when setting an address", new[] { nameof(BillingAddress) }));
            }

            return validationResults;
        }
    }
}
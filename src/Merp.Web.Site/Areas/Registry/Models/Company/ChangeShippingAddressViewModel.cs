using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class ChangeShippingAddressViewModel : IValidatableObject
    {
        [Required]
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
   
        [DisplayName("Shipping Address")]
        public PostalAddress ShippingAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (!ShippingAddress.IsValid)
            {
                validationResults.Add(new ValidationResult($"Invalid {nameof(ShippingAddress)}: {nameof(ShippingAddress.Address)} and {nameof(ShippingAddress.City)} are mandatory when setting an address", new[] { nameof(ShippingAddress) }));
            }

            return validationResults;
        }
    }
}
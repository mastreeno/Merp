using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Person
{
    public class ChangeAddressViewModel : IValidatableObject
    {
        [Required]
        public Guid PersonId { get; set; }
   
        [DisplayName("Address")]
        public PostalAddress Address { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Address.Address))
            {
                validationResults.Add(new ValidationResult($"{nameof(Address.Address)} is required", new[] { $"{nameof(Address)}.{nameof(Address.Address)}" }));
            }

            if (string.IsNullOrWhiteSpace(Address.City))
            {
                validationResults.Add(new ValidationResult($"{nameof(Address.City)} is required", new[] { $"{nameof(Address)}.{nameof(Address.City)}" }));
            }

            return validationResults;
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Api.Internal.Models.Party
{
    public class RegisterModel : IValidatableObject
    {
        public string FirstName { get; set; }

        [Required]
        public string LastNameOrCompanyName { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public string VatNumber { get; set; }

        public PostalAddress Address { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (!string.IsNullOrWhiteSpace(FirstName) && NationalIdentificationNumber.Length < 16)
            {
                result.Add(new ValidationResult("Invalid National identification number", new string[] { nameof(NationalIdentificationNumber) }));
            }

            return result;
        }
    }
}

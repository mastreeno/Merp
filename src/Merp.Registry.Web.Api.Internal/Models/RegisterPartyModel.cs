using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Api.Internal.Models
{
    public class RegisterPartyModel : IValidatableObject
    {
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastNameOrCompanyName { get; set; } = string.Empty;

        public string NationalIdentificationNumber { get; set; } = string.Empty;

        public string? VatNumber { get; set; }

        public PostalAddress Address { get; set; } = new();

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

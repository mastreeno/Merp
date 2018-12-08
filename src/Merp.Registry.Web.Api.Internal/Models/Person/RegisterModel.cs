using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Api.Internal.Models.Person
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [StringLength(16, MinimumLength = 16, ErrorMessage = "National Identification Number must have a length of 16 characters")]
        public string NationalIdentificationNumber { get; set; }

        public string VatNumber { get; set; }

        public PostalAddress Address { get; set; }
    }
}

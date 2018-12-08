using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Api.Internal.Models.Company
{
    public class RegisterModel
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string VatNumber { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public PostalAddress Address { get; set; }
    }
}

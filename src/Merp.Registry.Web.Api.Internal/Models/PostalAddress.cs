using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Api.Internal.Models
{
    public record PostalAddress
    {
        [RequiredIfNotEmpty(nameof(City), nameof(PostalCode), nameof(Province), nameof(Country))]
        public string Address { get; set; } = string.Empty;

        [RequiredIfNotEmpty(nameof(Address), nameof(PostalCode), nameof(Province), nameof(Country))]
        public string City { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
    }
}

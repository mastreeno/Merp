using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Api.Internal.Models
{
    public class PostalAddress
    {
        [RequiredIfNotEmpty(nameof(City), nameof(PostalCode), nameof(Province), nameof(Country))]
        public string Address { get; set; }

        [RequiredIfNotEmpty(nameof(Address), nameof(PostalCode), nameof(Province), nameof(Country))]
        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }
    }
}

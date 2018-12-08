using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Models
{
    public class PostalAddress
    {
        [RequiredIfNotEmpty(nameof(City), nameof(PostalCode), nameof(Province), nameof(Country))]
        [DisplayName("Address")]
        public string Address { get; set; }

        [RequiredIfNotEmpty(nameof(Address), nameof(PostalCode), nameof(Province), nameof(Country))]
        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [DisplayName("Province")]
        public string Province { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }
    }
}

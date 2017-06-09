using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models
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

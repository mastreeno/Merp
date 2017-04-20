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
        [DisplayName("Address")]
      
        public string Address { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [DisplayName("Province")]
        public string Province { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        public bool IsValid
        {
            get { return !(AnyFieldHasBeenSet() && !BothPrimaryFieldHasBeenSet()); }
        }

        private bool BothPrimaryFieldHasBeenSet()
        {
            return !string.IsNullOrWhiteSpace(Address) && !string.IsNullOrWhiteSpace(City);
        }

        private bool AnyFieldHasBeenSet()
        {
            return !string.IsNullOrWhiteSpace(Address)
                || !string.IsNullOrWhiteSpace(City)
                || !string.IsNullOrWhiteSpace(PostalCode) 
                || !string.IsNullOrWhiteSpace(Province) 
                || !string.IsNullOrWhiteSpace(Country);
        }
    }
}

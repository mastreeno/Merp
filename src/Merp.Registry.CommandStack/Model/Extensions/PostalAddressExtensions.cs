using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Model
{
    public static class PostalAddressExtensions
    {
        public static bool IsDifferentAddress(this PostalAddress postalAddress, string address, string city, string postalCode, string province, string country)
        {
            if(postalAddress == null)
            {
                throw new ArgumentNullException(nameof(postalAddress));
            }

            return postalAddress.Address != address
                || postalAddress.City != city
                || postalAddress.PostalCode != postalCode
                || postalAddress.Province != province
                || postalAddress.Country != country;
        }
    }
}

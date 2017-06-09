using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Helpers
{
    public static class PostalAddressHelper
    {
        public static bool IsValidAddress(string address, string city, string postalCode, string province, string country)
        {
            return (string.IsNullOrWhiteSpace(address) && string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(postalCode) && string.IsNullOrWhiteSpace(province) && string.IsNullOrWhiteSpace(country))
                ||
                (!string.IsNullOrWhiteSpace(address) && !string.IsNullOrWhiteSpace(city));
        }
    }
}

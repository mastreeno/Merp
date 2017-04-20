using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Model
{
    /// <summary>
    /// Represents a postal address
    /// </summary>
    public class PostalAddress
    {
        /// <summary>
        /// Gets or sets the address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the City
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the postal code
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// Gets or sets the province
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Creates an instance of an object describing a postal address
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="country">The country</param>
        public PostalAddress(string address, string city, string country)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("A valid address must be provided", nameof(address));
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("A valid country must be provided", nameof(city));
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("A valid city must be provided", nameof(country));
            Address = address;
            City = city;
            Country = country;
        }
    }
}

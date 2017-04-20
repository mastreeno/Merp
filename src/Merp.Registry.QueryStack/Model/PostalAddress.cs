using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.QueryStack.Model
{
    /// <summary>
    /// Represents a postal address
    /// </summary>
    [ComplexType]
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
    }
}

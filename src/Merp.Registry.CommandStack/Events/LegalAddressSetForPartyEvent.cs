using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Events
{
    /// <summary>
    /// Represents an occurred shipping address setting for an existing party
    /// </summary>
    public class LegalAddressSetForPartyEvent : DomainEvent
    {
        /// <summary>
        /// Gets or sets the Party Id
        /// </summary>
        public Guid PartyId { get; set; }
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
        /// Creates a new instance of the event
        /// </summary>
        /// <param name="partyId">The id of the party</param>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param> 
        public LegalAddressSetForPartyEvent(Guid partyId, string address, string city, string postalCode, string province, string country)
        {
            PartyId = partyId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            City = city ?? throw new ArgumentNullException(nameof(city));
            PostalCode = postalCode;
            Province = province;
            Country = country ?? throw new ArgumentNullException(nameof(country));
        }
    }
}

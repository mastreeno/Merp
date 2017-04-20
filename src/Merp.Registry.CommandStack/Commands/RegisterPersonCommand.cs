using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Commands
{
    public class RegisterPersonCommand : Command
    {
        /// <summary>
        /// Gets or sets the PersonId
        /// </summary>
        public Guid PersonId { get; set; }
        /// <summary>
        /// Gets or sets the First Name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets National Identification Number
        /// </summary>
        public string NationalIdentificationNumber { get; set; }
        /// <summary>
        /// Gets or sets the VAT number
        /// </summary>
        public string VatNumber { get; set; }
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
        /// Creates a new instance of the command
        /// </summary>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="nationalIdentificationNumber">The National Identification Number</param>
        /// <param name="vatNumber">The VAT numer</param>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param> 
        public RegisterPersonCommand(string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, string address, string city, string postalCode, string province, string country)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            NationalIdentificationNumber = nationalIdentificationNumber ?? throw new ArgumentNullException(nameof(nationalIdentificationNumber));
            VatNumber = vatNumber;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            City = city ?? throw new ArgumentNullException(nameof(city));
            PostalCode = postalCode;
            Province = province;
            Country = country;
        }
    }
}

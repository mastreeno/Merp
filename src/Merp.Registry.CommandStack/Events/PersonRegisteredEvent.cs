using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Events
{
    /// <summary>
    /// Represents an occurred person registration
    /// </summary>
    public class PersonRegisteredEvent : DomainEvent
    {
        /// <summary>
        /// Gets or sets the person Id
        /// </summary>
        public Guid PersonId { get; set; }
        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the National Identification Number
        /// </summary>
        public string NationalIdentificationNumber { get; set; }
        /// <summary>
        /// Gets or sets VAT Number
        /// </summary>
        public string VatNumber { get; set; }

        /// <summary>
        /// Creates an instance of the event
        /// </summary>
        /// <param name="personId">The person Id</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="nationalIdentificationNumber">The national identification number</param>
        /// <param name="vatNumber">The VAT number</param>
        public PersonRegisteredEvent(Guid personId, string firstName, string lastName, string nationalIdentificationNumber, string vatNumber)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            NationalIdentificationNumber = nationalIdentificationNumber;
            VatNumber = VatNumber;
        }
    }
}

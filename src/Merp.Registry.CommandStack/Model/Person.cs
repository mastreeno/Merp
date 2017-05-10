using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;
using Merp.Registry.CommandStack.Events;

namespace Merp.Registry.CommandStack.Model
{
    /// <summary>
    /// Person aggregate root
    /// </summary>
    public class Person : Party,
        IApplyEvent<PersonRegisteredEvent>
    {
        /// <summary>
        /// Gets the Person's first Name
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the Person's last name
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected Person()
        {

        }

        /// <summary>
        /// Apply an event to the current instance
        /// </summary>
        /// <param name="evt">The event</param>
        public void ApplyEvent(PersonRegisteredEvent evt)
        {
            Id = evt.PersonId;
            FirstName = evt.FirstName;
            LastName = evt.LastName;
            NationalIdentificationNumber = evt.NationalIdentificationNumber;
            VatNumber = evt.VatNumber;
        }

        /// <summary>
        /// Sets an address for the person
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param>
        public void SetAddress(string address, string city, string postalCode, string province, string country)
        {
            SetLegalAddress(address, city, postalCode, province, country);
            SetShippingAddress(address, city, postalCode, province, country);
            SetBillingAddress(address, city, postalCode, province, country);
        }

        /// <summary>
        /// Contains the Person aggregate factories
        /// </summary>
        public static class Factory
        {
            /// <summary>
            /// Creates an instance of the Person aggregate
            /// </summary>
            /// <param name="firstName">The person's first name</param>
            /// <param name="lastName">The person's last name</param>
            /// <param name="nationalIdentificationNumber">The person's National Identification Number</param>
            /// <param name="vatNumber">The person's VAT Number</param>
            /// <returns>The aggregate instance</returns>
            /// <exception cref="ArgumentException">Thrown if the firstName or the last name are null or empty</exception>
            public static Person CreateNewEntry(string firstName, string lastName, string nationalIdentificationNumber, string vatNumber)
            {
                var personId = Guid.NewGuid();
                return CreateNewEntryByImport(personId, firstName, lastName, nationalIdentificationNumber, vatNumber);
            }

            /// <summary>
            /// Creates an instance of the Person aggregate
            /// </summary>
            /// <param name="personId">The aggregate Id</param>
            /// <param name="firstName">The person's first name</param>
            /// <param name="lastName">The person's last name</param>
            /// <param name="nationalIdentificationNumber">The person's National Identification Number</param>
            /// <param name="vatNumber">The person's VAT Number</param>
            /// <returns>The aggregate instance</returns>
            /// <exception cref="ArgumentException">Thrown if the firstName or the last name are null or empty</exception>
            public static Person CreateNewEntryByImport(Guid personId, string firstName, string lastName, string nationalIdentificationNumber, string vatNumber)
            {
                if (string.IsNullOrWhiteSpace(firstName))
                    throw new ArgumentException("The first name must be specified", nameof(firstName));
                if (string.IsNullOrWhiteSpace(lastName))
                    throw new ArgumentException("The last name must be specified", nameof(lastName));
                if (string.IsNullOrWhiteSpace(nationalIdentificationNumber))
                    throw new ArgumentException("The National Identification Number", nameof(nationalIdentificationNumber));

                var e = new PersonRegisteredEvent(personId, firstName, lastName, nationalIdentificationNumber, vatNumber);
                var p = new Person();
                p.RaiseEvent(e);
                return p;
            }
        }
    }
}

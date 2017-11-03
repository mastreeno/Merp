using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using MementoFX.Domain;
using Merp.Registry.CommandStack.Events;
using Merp.Registry.CommandStack.Helpers;

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
            RegistrationDate = evt.TimeStamp;
            if (!string.IsNullOrWhiteSpace(evt.Address) && !string.IsNullOrWhiteSpace(evt.City) && !string.IsNullOrWhiteSpace(evt.Country))
            {
                var legalAddress = new PostalAddress(evt.Address, evt.City, evt.Country)
                {
                    PostalCode = evt.PostalCode,
                    Province = evt.Province
                };
                var shippingAddress = new PostalAddress(evt.Address, evt.City, evt.Country)
                {
                    PostalCode = evt.PostalCode,
                    Province = evt.Province
                };
                var billingAddress = new PostalAddress(evt.Address, evt.City, evt.Country)
                {
                    PostalCode = evt.PostalCode,
                    Province = evt.Province
                };
                this.LegalAddress = legalAddress;
                this.ShippingAddress = shippingAddress;
                this.BillingAddress = billingAddress;
            }
            var contactInfo = new ContactInfo(evt.PhoneNumber, evt.MobileNumber, evt.FaxNumber, evt.WebsiteAddress, evt.EmailAddress, evt.InstantMessaging, evt.Pec);
            this.ContactInfo = contactInfo;
        }

        /// <summary>
        /// Sets an address for the person
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param>
        /// <param name="effectiveDate">The effective date</param>
        public void ChangeAddress(string address, string city, string postalCode, string province, string country, DateTime effectiveDate)
        {
            ChangeLegalAddress(address, city, postalCode, province, country, effectiveDate);
            ChangeShippingAddress(address, city, postalCode, province, country, effectiveDate);
            ChangeBillingAddress(address, city, postalCode, province, country, effectiveDate);
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
            public static Person CreateNewEntry(string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, string address, string city, string postalCode, string province, string country, string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging, string pec)
            {
                if (string.IsNullOrWhiteSpace(firstName))
                    throw new ArgumentException("The first name must be specified", nameof(firstName));

                if (string.IsNullOrWhiteSpace(lastName))
                    throw new ArgumentException("The last name must be specified", nameof(lastName));

                if (!string.IsNullOrWhiteSpace(nationalIdentificationNumber))
                {
                    if (!NationalIdentificationNumberHelper.IsMatchingFirstName(nationalIdentificationNumber, firstName))
                    {
                        throw new ArgumentException("National identification number is not matching with first name", nameof(nationalIdentificationNumber));
                    }
                    if (!NationalIdentificationNumberHelper.IsMatchingLastName(nationalIdentificationNumber, lastName))
                    {
                        throw new ArgumentException("National identification number is not matching with last name", nameof(nationalIdentificationNumber));
                    }
                }
                if (!PostalAddressHelper.IsValidAddress(address, city, postalCode, province, country))
                {
                    throw new ArgumentException("postal address must either be empty or comprehensive of both address and city");
                }

                var personId = Guid.NewGuid();
                var registrationDate = DateTime.Now;
                var e = new PersonRegisteredEvent(personId, registrationDate, firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging, pec);
                var p = new Person();
                p.RaiseEvent(e);
                return p;
            }

            /// <summary>
            /// Creates an instance of the Person aggregate
            /// </summary>
            /// <param name="personId">The aggregate Id</param>
            /// <param name="registrationDate">The date in which the person was registered</param>
            /// <param name="firstName">The person's first name</param>
            /// <param name="lastName">The person's last name</param>
            /// <param name="nationalIdentificationNumber">The person's National Identification Number</param>
            /// <param name="vatNumber">The person's VAT Number</param>
            /// <returns>The aggregate instance</returns>
            /// <exception cref="ArgumentException">Thrown if the firstName or the last name are null or empty</exception>
            public static Person CreateNewEntryByImport(Guid personId, DateTime registrationDate, string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, string address, string city, string postalCode, string province, string country, string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging, string pec)
            {
                if (string.IsNullOrWhiteSpace(firstName))
                    throw new ArgumentException("The first name must be specified", nameof(firstName));

                if (string.IsNullOrWhiteSpace(lastName))
                    throw new ArgumentException("The last name must be specified", nameof(lastName));

                //if (!string.IsNullOrWhiteSpace(nationalIdentificationNumber))
                //{
                //    if (!NationalIdentificationNumberHelper.IsMatchingFirstName(nationalIdentificationNumber, firstName))
                //    {
                //        throw new ArgumentException("National identification number is not matching with first name", nameof(nationalIdentificationNumber));
                //    }
                //    if (!NationalIdentificationNumberHelper.IsMatchingLastName(nationalIdentificationNumber, lastName))
                //    {
                //        throw new ArgumentException("National identification number is not matching with last name", nameof(nationalIdentificationNumber));
                //    }
                //}
                if (!PostalAddressHelper.IsValidAddress(address, city, postalCode, province, country))
                {
                    throw new ArgumentException("postal address must either be empty or comprehensive of both address and city");
                }

                var e = new PersonRegisteredEvent(personId, registrationDate, firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging, pec);
                var p = new Person();
                p.RaiseEvent(e);
                return p;
            }
        }
    }
}

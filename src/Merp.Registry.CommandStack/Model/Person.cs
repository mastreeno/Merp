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
            if (!string.IsNullOrWhiteSpace(evt.LegalAddressAddress) && !string.IsNullOrWhiteSpace(evt.LegalAddressCity) && !string.IsNullOrWhiteSpace(evt.LegalAddressCountry))
            {
                var legalAddress = new PostalAddress(evt.LegalAddressAddress, evt.LegalAddressCity, evt.LegalAddressCountry)
                {
                    PostalCode = evt.LegalAddressPostalCode,
                    Province = evt.LegalAddressProvince
                };
                this.LegalAddress = legalAddress;
            }
            if (!string.IsNullOrWhiteSpace(evt.ShippingAddressAddress) && !string.IsNullOrWhiteSpace(evt.ShippingAddressCity) && !string.IsNullOrWhiteSpace(evt.ShippingAddressCountry))
            {
                var shippingAddress = new PostalAddress(evt.ShippingAddressAddress, evt.ShippingAddressCity, evt.ShippingAddressCountry)
                {
                    PostalCode = evt.ShippingAddressPostalCode,
                    Province = evt.ShippingAddressProvince
                };
                this.ShippingAddress = shippingAddress;
            }
            if (!string.IsNullOrWhiteSpace(evt.BillingAddressAddress) && !string.IsNullOrWhiteSpace(evt.BillingAddressCity) && !string.IsNullOrWhiteSpace(evt.BillingAddressCountry))
            {
                var billingAddress = new PostalAddress(evt.BillingAddressAddress, evt.BillingAddressCity, evt.BillingAddressCountry)
                {
                    PostalCode = evt.BillingAddressPostalCode,
                    Province = evt.BillingAddressProvince
                };
                this.BillingAddress = billingAddress;
            }
            var contactInfo = new ContactInfo(evt.PhoneNumber, evt.MobileNumber, evt.FaxNumber, evt.WebsiteAddress, evt.EmailAddress, evt.InstantMessaging);
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
            ChangeLegalAddress(address, city, postalCode, province, country, effectiveDate, Guid.Empty);
            ChangeShippingAddress(address, city, postalCode, province, country, effectiveDate, Guid.Empty);
            ChangeBillingAddress(address, city, postalCode, province, country, effectiveDate, Guid.Empty);
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
            public static Person CreateNewEntry(string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, 
                string legalAddressAddress, string legalAddressCity, string legalAddressPostalCode, string legalAddressProvince, string legalAddressCountry,
                string billingAddressAddress, string billingAddressCity, string billingAddressPostalCode, string billingAddressProvince, string billingAddressCountry,
                string shippingAddressAddress, string shippingAddressCity, string shippingAddressPostalCode, string shippingAddressProvince, string shippingAddressCountry,
                string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging, Guid userId)
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
                if (!PostalAddressHelper.IsValidAddress(legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince, legalAddressCountry))
                {
                    throw new ArgumentException("legal address must either be empty or comprehensive of both address and city");
                }

                if (!PostalAddressHelper.IsValidAddress(shippingAddressAddress, shippingAddressCity, shippingAddressPostalCode, shippingAddressProvince, shippingAddressCountry))
                {
                    throw new ArgumentException("shipping address must either be empty or comprehensive of both address and city");
                }

                if (!PostalAddressHelper.IsValidAddress(billingAddressAddress, billingAddressCity, billingAddressPostalCode, billingAddressProvince, billingAddressCountry))
                {
                    throw new ArgumentException("billing address must either be empty or comprehensive of both address and city");
                }

                var personId = Guid.NewGuid();
                var registrationDate = DateTime.Now;
                var e = new PersonRegisteredEvent(personId, registrationDate, firstName, lastName, nationalIdentificationNumber, vatNumber, 
                    legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince, legalAddressCountry,
                    billingAddressAddress, billingAddressCity, billingAddressPostalCode, billingAddressProvince, billingAddressCountry,
                    shippingAddressAddress, shippingAddressCity, shippingAddressPostalCode, shippingAddressProvince, shippingAddressCountry,
                    phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging, userId);
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
            public static Person CreateNewEntryByImport(Guid personId, DateTime registrationDate, string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, string address, string city, string postalCode, string province, string country, string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging, Guid userId)
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

                var e = new PersonRegisteredEvent(personId, registrationDate, firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, address, city, postalCode, province, country, address, city, postalCode, province, country, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging, userId);
                var p = new Person();
                p.RaiseEvent(e);
                return p;
            }
        }
    }
}

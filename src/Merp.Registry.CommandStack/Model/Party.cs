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
    /// Represents a party
    /// </summary>
    public class Party : Aggregate,
        IApplyEvent<PartyLegalAddressChangedEvent>,
        IApplyEvent<PartyShippingAddressChangedEvent>,
        IApplyEvent<PartyBillingAddressChangedEvent>,
        IApplyEvent<ContactInfoSetForPartyEvent>
    {
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets National Identification Number
        /// </summary>
        public string NationalIdentificationNumber { get; protected set; }
        
        /// <summary>
        /// Gets or sets the VAT index
        /// </summary>
        public string VatNumber { get; protected set; }

        /// <summary>
        /// Gets or sets the legal address
        /// </summary>
        public PostalAddress LegalAddress { get; protected set; }

        /// <summary>
        /// Gets or sets the shipping address
        /// </summary>
        public PostalAddress ShippingAddress { get; protected set; }

        /// <summary>
        /// Gets or sets the billing address
        /// </summary>
        public PostalAddress BillingAddress { get; protected set; }

        /// <summary>
        /// Gets or sets the contact info
        /// </summary>
        public ContactInfo ContactInfo { get; protected set; }

        /// <summary>
        /// Apply an event to the current instance
        /// </summary>
        /// <param name="evt">The event</param>
        public void ApplyEvent([AggregateId(nameof(PartyShippingAddressChangedEvent.PartyId))] PartyShippingAddressChangedEvent evt)
        {
            var shippingAddress = new PostalAddress(evt.Address, evt.City, evt.Country)
            {
                PostalCode = evt.PostalCode,
                Province = evt.Province
            };
            this.ShippingAddress = shippingAddress;
        }

        /// <summary>
        /// Apply an event to the current instance
        /// </summary>
        /// <param name="evt">The event</param>
        public void ApplyEvent([AggregateId(nameof(PartyLegalAddressChangedEvent.PartyId))] PartyLegalAddressChangedEvent evt)
        {            
            var legalAddress = new PostalAddress(evt.Address, evt.City, evt.Country)
            {
                PostalCode = evt.PostalCode,
                Province = evt.Province
            };
            this.LegalAddress = legalAddress;
        }

        /// <summary>
        /// Apply an event to the current instance
        /// </summary>
        /// <param name="evt">The event</param>
        public void ApplyEvent([AggregateId(nameof(PartyBillingAddressChangedEvent.PartyId))] PartyBillingAddressChangedEvent evt)
        {
            var billingAddress = new PostalAddress(evt.Address, evt.City, evt.Country)
            {
                PostalCode = evt.PostalCode,
                Province = evt.Province
            };
            this.BillingAddress = billingAddress;
        }

        /// <summary>
        /// Apply an event to the current instance
        /// </summary>
        /// <param name="evt">The event</param>
        public void ApplyEvent([AggregateId(nameof(ContactInfoSetForPartyEvent.PartyId))] ContactInfoSetForPartyEvent evt)
        {
            var contactInfo = new ContactInfo(evt.PhoneNumber, evt.MobileNumber, evt.FaxNumber, evt.WebsiteAddress, evt.EmailAddress, evt.InstantMessaging);
            this.ContactInfo = contactInfo;
        }

        /// <summary>
        /// Sets legal address for the party
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param>
        /// <param name="effectiveDate">The country</param>
        public void ChangeLegalAddress(string address, string city, string postalCode, string province, string country, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                throw new InvalidOperationException("A valid address, city and country must be provided");
            }
            if (effectiveDate > DateTime.Now)
            {
                throw new ArgumentException("The legal address change cannot be scheduled in the future", nameof(effectiveDate));
            }
            if(effectiveDate < RegistrationDate.ToLocalTime())
            {
                throw new ArgumentException("Cannot change the legal address to an effective date before the registration date", nameof(effectiveDate));
            }

            var e = new PartyLegalAddressChangedEvent(Id, address, city, postalCode, province, country, effectiveDate);            
            RaiseEvent(e);
        }

        /// <summary>
        /// Sets shipping address for the party
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param>
        /// <param name="effectiveDate">The country</param>
        public void ChangeShippingAddress(string address, string city, string postalCode, string province, string country, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                throw new InvalidOperationException("A valid address, city and country must be provided");
            }
            if (effectiveDate < RegistrationDate.ToLocalTime())
            {
                throw new ArgumentException("Cannot change the shipping address to an effective date before the registration date", nameof(effectiveDate));
            }

            var e = new PartyShippingAddressChangedEvent(Id, address, city, postalCode, province, country, effectiveDate);
            RaiseEvent(e);
        }

        /// <summary>
        /// Sets billing address for the party
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param>
        /// <param name="effectiveDate">The country</param>
        public void ChangeBillingAddress(string address, string city, string postalCode, string province, string country, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                throw new InvalidOperationException("A valid address, city and country must be provided");
            }
            if (effectiveDate < RegistrationDate.ToLocalTime())
            {
                throw new ArgumentException("Cannot change the billing address to an effective date before the registration date", nameof(effectiveDate));
            }

            var e = new PartyBillingAddressChangedEvent(Id, address, city, postalCode, province, country, effectiveDate);
            RaiseEvent(e);
        }

        /// <summary>
        /// Sets contact info for the party
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="mobileNumber"></param>
        /// <param name="faxNumber"></param>
        /// <param name="websiteAddress"></param>
        /// <param name="emailAddress"></param>
        /// <param name="instantMessaging"></param>
        public void SetContactInfo(string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging)
        {     
            var e = new ContactInfoSetForPartyEvent(Id, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging);
            RaiseEvent(e);
        }
    }
}

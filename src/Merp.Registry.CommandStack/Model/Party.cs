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
        IApplyEvent<LegalAddressSetForPartyEvent>,
        IApplyEvent<ShippingAddressSetForPartyEvent>,
        IApplyEvent<BillingAddressSetForPartyEvent>
    {
        /// <summary>
        /// Gets or sets National Identification Number
        /// </summary>
        public string NationalIdentificationNumber { get; protected set; }
        
        /// <summary>
        /// Gets or sets the VAT index
        /// </summary>
        public string VatIndex { get; protected set; }

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
        /// Apply an event to the current instance
        /// </summary>
        /// <param name="evt">The event</param>
        public void ApplyEvent([AggregateId(nameof(ShippingAddressSetForPartyEvent.PartyId))] ShippingAddressSetForPartyEvent evt)
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
        public void ApplyEvent([AggregateId(nameof(LegalAddressSetForPartyEvent.PartyId))] LegalAddressSetForPartyEvent evt)
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
        public void ApplyEvent([AggregateId(nameof(BillingAddressSetForPartyEvent.PartyId))] BillingAddressSetForPartyEvent evt)
        {
            var billingAddress = new PostalAddress(evt.Address, evt.City, evt.Country)
            {
                PostalCode = evt.PostalCode,
                Province = evt.Province
            };
            this.BillingAddress = billingAddress;
        }

        /// <summary>
        /// Sets an address for the person
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param>
        public void SetLegalAddress(string address, string city, string postalCode, string province, string country)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
                throw new InvalidOperationException("A valid address, city and country must be provided");

            var e = new LegalAddressSetForPartyEvent(Id, address, city, postalCode, province, country);            
            RaiseEvent(e);
        }

        /// <summary>
        /// Sets an address for the person
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param>
        public void SetShippingAddress(string address, string city, string postalCode, string province, string country)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
                throw new InvalidOperationException("A valid address, city and country must be provided");

            var e = new ShippingAddressSetForPartyEvent(Id, address, city, postalCode, province, country);
            RaiseEvent(e);
        }

        /// <summary>
        /// Sets an address for the person
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="city">The city</param>
        /// <param name="postalCode">The postal code</param>
        /// <param name="province">The province</param>
        /// <param name="country">The country</param>
        public void SetBillingAddress(string address, string city, string postalCode, string province, string country)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
                throw new InvalidOperationException("A valid address, city and country must be provided");

            var e = new BillingAddressSetForPartyEvent(Id, address, city, postalCode, province, country);
            RaiseEvent(e);
        }
    }
}

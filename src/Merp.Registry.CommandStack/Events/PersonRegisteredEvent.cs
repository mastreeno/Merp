using MementoFX.Domain;
using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Events
{
    public class PersonRegisteredEvent : MerpDomainEvent
    {
        public Guid PersonId { get; set; }
        [Timestamp]
        public DateTime RegistrationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalIdentificationNumber { get; set; }
        public string VatNumber { get; set; }
        public string LegalAddressAddress { get; set; }
        public string LegalAddressCity { get; set; }
        public string LegalAddressPostalCode { get; set; }
        public string LegalAddressProvince { get; set; }
        public string LegalAddressCountry { get; set; }
        public string ShippingAddressAddress { get; set; }
        public string ShippingAddressPostalCode { get; set; }
        public string ShippingAddressCity { get; set; }
        public string ShippingAddressCountry { get; set; }
        public string ShippingAddressProvince { get; set; }
        public string BillingAddressAddress { get; set; }
        public string BillingAddressPostalCode { get; set; }
        public string BillingAddressCity { get; set; }
        public string BillingAddressCountry { get; set; }
        public string BillingAddressProvince { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }
        public string InstantMessaging { get; set; }

        public PersonRegisteredEvent(Guid personId, DateTime registrationDate, string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, 
            string legalAddressAddress, string legalAddressCity, string legalAddressPostalCode, string legalAddressProvince, string legalAddressCountry,
            string billingAddressAddress, string billingAddressCity, string billingAddressPostalCode, string billingAddressProvince, string billingAddressCountry,
            string shippingAddressAddress, string shippingAddressCity, string shippingAddressPostalCode, string shippingAddressProvince, string shippingAddressCountry,
            string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging, Guid userId)
            : base(userId)
        {
            PersonId = personId;
            RegistrationDate = registrationDate;
            FirstName = firstName;
            LastName = lastName;
            NationalIdentificationNumber = nationalIdentificationNumber;
            VatNumber = vatNumber;
            LegalAddressAddress = legalAddressAddress;
            LegalAddressCity = legalAddressCity;
            LegalAddressPostalCode = legalAddressPostalCode;
            LegalAddressProvince = legalAddressProvince;
            LegalAddressCountry = legalAddressCountry;
            BillingAddressAddress = billingAddressAddress;
            BillingAddressCity = billingAddressCity;
            BillingAddressPostalCode = billingAddressPostalCode;
            BillingAddressProvince = billingAddressProvince;
            BillingAddressCountry = billingAddressCountry;
            ShippingAddressAddress = shippingAddressAddress;
            ShippingAddressCity = shippingAddressCity;
            ShippingAddressPostalCode = shippingAddressPostalCode;
            ShippingAddressProvince = shippingAddressProvince;
            ShippingAddressCountry = shippingAddressCountry;
            PhoneNumber = phoneNumber;
            MobileNumber = mobileNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
            InstantMessaging = instantMessaging;
        }
    }
}

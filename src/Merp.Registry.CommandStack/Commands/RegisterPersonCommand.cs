using Merp.Domain;
using Merp.Registry.CommandStack.Helpers;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class RegisterPersonCommand : MerpCommand
    {
        public Guid PersonId { get; set; }
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

        public RegisterPersonCommand(Guid userId, string firstName, string lastName, string nationalIdentificationNumber, string vatNumber, string legalAddressAddress, string legalAddressCity, string legalAddressPostalCode, string legalAddressProvince, string legalAddressCountry, string shippingAddressAddress, string shippingAddressPostalCode, string shippingAddressCity, string shippingAddressProvince, string shippingAddressCountry, string billingAddressAddress, string billingAddressPostalCode, string billingAddressCity, string billingAddressProvince, string billingAddressCountry, string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging)
            : base(userId)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name must be provided", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name must be provided", nameof(lastName));
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
            FirstName = firstName;
            LastName = lastName;
            NationalIdentificationNumber = nationalIdentificationNumber;
            VatNumber = vatNumber;
            LegalAddressAddress = legalAddressAddress;
            LegalAddressCity = legalAddressCity ;
            LegalAddressPostalCode = legalAddressPostalCode;
            LegalAddressProvince = legalAddressProvince;
            LegalAddressCountry = legalAddressCountry;
            ShippingAddressAddress = shippingAddressAddress;
            ShippingAddressPostalCode = shippingAddressPostalCode;
            ShippingAddressCity = shippingAddressCity;
            ShippingAddressProvince = shippingAddressProvince;
            ShippingAddressCountry = shippingAddressCountry;
            BillingAddressAddress = billingAddressAddress;
            BillingAddressPostalCode = billingAddressPostalCode;
            BillingAddressCity = billingAddressCity;
            BillingAddressProvince = billingAddressProvince;
            BillingAddressCountry = billingAddressCountry;
            PhoneNumber = phoneNumber;
            MobileNumber = mobileNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
            InstantMessaging = instantMessaging;
        }
    }
}

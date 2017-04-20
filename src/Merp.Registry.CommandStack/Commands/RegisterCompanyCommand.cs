using Memento;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class RegisterCompanyCommand : Command
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
        public string NationalIdentificationNumber { get; set; }

        public string LegalAddressAddress { get; set; }
        public string LegalAddressPostalCode { get; set; }
        public string LegalAddressCity { get; set; }
        public string LegalAddressCountry { get; set; }
        public string LegalAddressProvince { get; set; }

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

        public RegisterCompanyCommand(string companyName, string nationalIdentificationNumber, string vatNumber, string legalAddressAddress, string legalAddressPostalCode, string legalAddressCity, string legalAddressProvince, string legalAddressCountry,  string shippingAddressAddress, string shippingAddressPostalCode, string shippingAddressCity, string shippingAddressProvince, string shippingAddressCountry, string billingAddressAddress, string billingAddressPostalCode, string billingAddressCity, string billingAddressProvince, string billingAddressCountry)
        {
            CompanyName = companyName ?? throw new ArgumentNullException(nameof(companyName));
            VatNumber = vatNumber ?? throw new ArgumentNullException(nameof(vatNumber));
            NationalIdentificationNumber = nationalIdentificationNumber;

            LegalAddressAddress = legalAddressAddress ?? throw new ArgumentNullException(nameof(legalAddressAddress));
            LegalAddressPostalCode = legalAddressPostalCode;
            LegalAddressCity = legalAddressCity ?? throw new ArgumentNullException(nameof(legalAddressCity));
            LegalAddressCountry = legalAddressCountry;
            LegalAddressProvince = legalAddressProvince;

            ShippingAddressAddress = shippingAddressAddress ?? throw new ArgumentNullException(nameof(shippingAddressAddress));
            ShippingAddressPostalCode = shippingAddressPostalCode;
            ShippingAddressCity = shippingAddressCity ?? throw new ArgumentNullException(nameof(shippingAddressCity));
            ShippingAddressCountry = shippingAddressCountry;
            ShippingAddressProvince = shippingAddressProvince;

            BillingAddressAddress = billingAddressAddress ?? throw new ArgumentNullException(nameof(billingAddressAddress));
            BillingAddressPostalCode = billingAddressPostalCode;
            BillingAddressCity = billingAddressCity ?? throw new ArgumentNullException(nameof(billingAddressCity));
            BillingAddressCountry = billingAddressCountry;
            BillingAddressProvince = billingAddressProvince;
        }
    }
}
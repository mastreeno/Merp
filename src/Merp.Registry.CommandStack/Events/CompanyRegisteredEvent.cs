using MementoFX;
using MementoFX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Events
{
    public class CompanyRegisteredEvent : DomainEvent
    {
        public Guid CompanyId { get; set; }
        [Timestamp]
        public DateTime RegistrationDate { get; set; }
        public string CompanyName { get; set; }
        public string VatIndex { get; set; }
        public string NationalIdentificationNumber { get; set; }
        
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

        public CompanyRegisteredEvent(Guid companyId, DateTime registrationDate, string companyName, string vatIndex, string nationalIdentificationNumber,
            string legalAddressAddress, string legalAddressCity, string legalAddressPostalCode, string legalAddressProvince, string legalAddressCountry,
            string billingAddressAddress, string billingAddressCity, string billingAddressPostalCode, string billingAddressProvince, string billingAddressCountry,
            string shippingAddressAddress, string shippingAddressCity, string shippingAddressPostalCode, string shippingAddressProvince, string shippingAddressCountry)
        {
            CompanyId = companyId;
            RegistrationDate = registrationDate;
            CompanyName = companyName;
            VatIndex = vatIndex;
            NationalIdentificationNumber = nationalIdentificationNumber;

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
        }
    }
}

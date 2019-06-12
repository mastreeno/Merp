using MementoFX;
using Merp.Domain;
using Merp.Registry.CommandStack.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Commands
{
    /// <summary>
    /// The command which allows to import a person from an external source
    /// </summary>
    public class ImportCompanyCommand : MerpCommand
    {
        public Guid CompanyId { get; set; }
        public DateTime RegistrationDate { get; set; }
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

        public Guid? MainContactId { get; set; }
        public Guid? AdministrativeContactId { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }

        public ImportCompanyCommand(Guid userId, Guid companyId, DateTime registrationDate, string companyName, string nationalIdentificationNumber, string vatNumber, string legalAddressAddress, string legalAddressPostalCode, string legalAddressCity, string legalAddressProvince, string legalAddressCountry, string shippingAddressAddress, string shippingAddressPostalCode, string shippingAddressCity, string shippingAddressProvince, string shippingAddressCountry, string billingAddressAddress, string billingAddressPostalCode, string billingAddressCity, string billingAddressProvince, string billingAddressCountry, Guid? mainContactId, Guid? administrativeContactId, string phoneNumber, string faxNumber, string websiteAddress, string emailAddress)
            : base(userId)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("A non-empty companyId must be provided.", nameof(companyId));

            if (string.IsNullOrWhiteSpace(companyName))
                throw new ArgumentException("Company name must be provided", nameof(companyName));

            if (string.IsNullOrWhiteSpace(vatNumber) && string.IsNullOrWhiteSpace(nationalIdentificationNumber))
                throw new ArgumentException("Either the VAT number or the NIN must be provided", nameof(vatNumber));

            if (!PostalAddressHelper.IsValidAddress(legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince, legalAddressCountry))
                throw new ArgumentException("legal address must either be empty or comprehensive of both address and city");

            if (!PostalAddressHelper.IsValidAddress(shippingAddressAddress, shippingAddressCity, shippingAddressPostalCode, shippingAddressProvince, shippingAddressCountry))
                throw new ArgumentException("shipping address must either be empty or comprehensive of both address and city");

            if (!PostalAddressHelper.IsValidAddress(billingAddressAddress, billingAddressCity, billingAddressPostalCode, billingAddressProvince, billingAddressCountry))
                throw new ArgumentException("billing address must either be empty or comprehensive of both address and city");

            CompanyId = companyId;
            RegistrationDate = registrationDate;
            CompanyName = companyName;
            VatNumber = vatNumber;
            NationalIdentificationNumber = nationalIdentificationNumber;

            LegalAddressAddress = legalAddressAddress;
            LegalAddressPostalCode = legalAddressPostalCode;
            LegalAddressCity = legalAddressCity;
            LegalAddressCountry = legalAddressCountry;
            LegalAddressProvince = legalAddressProvince;

            ShippingAddressAddress = shippingAddressAddress;
            ShippingAddressPostalCode = shippingAddressPostalCode;
            ShippingAddressCity = shippingAddressCity;
            ShippingAddressCountry = shippingAddressCountry;
            ShippingAddressProvince = shippingAddressProvince;

            BillingAddressAddress = billingAddressAddress;
            BillingAddressPostalCode = billingAddressPostalCode;
            BillingAddressCity = billingAddressCity;
            BillingAddressCountry = billingAddressCountry;
            BillingAddressProvince = billingAddressProvince;

            MainContactId = mainContactId;
            AdministrativeContactId = administrativeContactId;
            PhoneNumber = phoneNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
        }

    }
}

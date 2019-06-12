using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Api.Public.Models
{
    public class ImportCompanyModel
    {
        public DateTime RegistrationDate { get; set; }

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

        public Guid? MainContactId { get; set; }
        public Guid? AdministrativeContactId { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }

        public Guid UserId { get; set; }
    }
}

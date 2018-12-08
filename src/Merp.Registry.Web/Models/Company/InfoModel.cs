using System;
using System.ComponentModel;

namespace Merp.Registry.Web.Models.Company
{
    public class InfoModel
    {
        public Guid CompanyUid { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [DisplayName("Vat Number")]
        public string VatNumber { get; set; }

        [DisplayName("National Identification Number")]
        public string NationalIdentificationNumber { get; set; }

        [DisplayName("Legal Address")]
        public PostalAddress LegalAddress { get; set; }

        [DisplayName("Shipping Address")]
        public PostalAddress ShippingAddress { get; set; }

        [DisplayName("Billing Address")]
        public PostalAddress BillingAddress { get; set; }

        public PersonInfo MainContact { get; set; }

        public PersonInfo AdministrativeContact { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Fax Number")]
        public string FaxNumber { get; set; }
        [DisplayName("Website Address")]
        public string WebsiteAddress { get; set; }
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
    }
}

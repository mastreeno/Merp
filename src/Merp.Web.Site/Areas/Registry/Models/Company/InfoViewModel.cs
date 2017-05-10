using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class InfoViewModel
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
        [DisplayName("Main Contact")]
        public string MainContactName { get; set; }
        [DisplayName("Administrative Contact")]
        public string AdministrativeContactName { get; set; }
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

using System;
using System.ComponentModel;

namespace Merp.Registry.Web.Models.Person
{
    public class InfoModel
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        public Guid OriginalId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("National Identification Number")]
        public string NationalIdentificationNumber { get; set; }
        [DisplayName("VAT number")]
        public string VatNumber { get; set; }
        [DisplayName("Legal Address")]
        public PostalAddress LegalAddress { get; set; }
        [DisplayName("Shipping Address")]
        public PostalAddress ShippingAddress { get; set; }
        [DisplayName("Billing Address")]
        public PostalAddress BillingAddress { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        [DisplayName("Fax Number")]
        public string FaxNumber { get; set; }
        [DisplayName("Website Address")]
        public string WebsiteAddress { get; set; }
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
        [DisplayName("IM")]
        public string InstantMessaging { get; set; }
    }
}

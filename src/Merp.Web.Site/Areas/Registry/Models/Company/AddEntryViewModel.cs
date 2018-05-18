using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class AddEntryViewModel
    {
        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [DisplayName("Vat Number")]
        public string VatNumber { get; set; }

        //[Required]
        [DisplayName("National Identification Number")]
        public string NationalIdentificationNumber { get; set; }

        [DisplayName("Legal Address")]
        public PostalAddress LegalAddress { get; set; }

        [Required]
        [DisplayName("Use Legal Address")]
        public bool AcquireBillingAddressFromLegalAddress { get; set; }

        [DisplayName("Billing Address")]
        public PostalAddress BillingAddress { get; set; }

        [Required]
        [DisplayName("Use Legal Address")]
        public bool AcquireShippingAddressFromLegalAddress { get; set; }

        [DisplayName("Shipping Address")]
        public PostalAddress ShippingAddress { get; set; }

        [DisplayName("Main Contact")]
        public PersonInfo MainContact { get; set; }

        [DisplayName("Administrative Contact")]
        public PersonInfo AdministrativeContact { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Fax Number")]
        public string FaxNumber { get; set; }

        [DisplayName("Website Address")]
        public string WebsiteAddress { get; set; }

        [DisplayName("Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [StringLength(40, MinimumLength = 5)]
        [DisplayName("Skype username")]
        public string Skype { get; set; }
    }
}
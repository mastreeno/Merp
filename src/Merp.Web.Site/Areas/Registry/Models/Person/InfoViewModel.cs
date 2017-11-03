using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models.Person
{
    public class InfoViewModel
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
        [DisplayName("Address")]
        public PostalAddress Address { get; set; }
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
        [DisplayName("PEC")]
        public string Pec { get; set; }
    }
}

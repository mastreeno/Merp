using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Models.Person
{
    public class ChangeContactInfoModel
    {
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        [DisplayName("Fax Number")]
        public string FaxNumber { get; set; }
        [DisplayName("Website Address")]
        public string WebsiteAddress { get; set; }
        [DisplayName("Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [DisplayName("IM")]
        public string InstantMessaging { get; set; }
    }
}

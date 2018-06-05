using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class ChangeContactInfoViewModel
    {
        [Required]
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Fax Number")]
        public string FaxNumber { get; set; }
        [DisplayName("Website Address")]
        public string WebsiteAddress { get; set; }
        [DisplayName("Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class AssociateMainContactViewModel
    {
        [Required]
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }

        [DisplayName("Administrative Contact")]
        public PersonInfo MainContact { get; set; }
    }
}

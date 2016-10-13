using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class ChangeNameViewModel
    {

        [Required]
        public Guid CompanyUid { get; set; }

        public string CurrentCompanyName { get; set; }

        [Required]
        public string NewCompanyName { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }
    }
}

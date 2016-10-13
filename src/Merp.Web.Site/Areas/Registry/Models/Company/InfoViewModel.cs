using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class InfoViewModel
    {
        public Guid CompanyUid { get; set; }
        public string CompanyName { get; set; }
        public string VatIndex { get; set; }
    }
}

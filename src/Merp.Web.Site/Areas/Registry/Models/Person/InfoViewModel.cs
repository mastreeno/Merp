using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models.Person
{
    public class InfoViewModel
    {
        public Guid PersonUid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

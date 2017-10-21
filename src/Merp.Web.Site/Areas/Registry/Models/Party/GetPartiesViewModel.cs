using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Party
{
    public class GetPartiesViewModel
    {
        public int id { get; set; }

        public Guid uid { get; set; }

        public string name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
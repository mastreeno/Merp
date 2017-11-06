using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Party
{
    public class GetPartiesViewModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Linkedin { get; set; }
    }
}
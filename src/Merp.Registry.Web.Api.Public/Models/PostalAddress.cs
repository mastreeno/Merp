using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Api.Public.Models
{
    public class PostalAddress
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }
    }
}

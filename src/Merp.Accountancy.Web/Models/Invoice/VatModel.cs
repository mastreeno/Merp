using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Models.Invoice
{
    public class VatModel
    {
        public decimal Rate { get; set; }

        public string Description { get; set; }
    }
}

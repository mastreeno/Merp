using System;
using System.Collections.Generic;

namespace Merp.Accountancy.Web.Api.Internal.Models.Vat
{
    public class ListModel
    {
        public int TotalNumberOfVats { get; set; }

        public IEnumerable<VatItem> Vats { get; set; }

        public class VatItem
        {
            public Guid Id { get; set; }

            public decimal Rate { get; set; }

            public string Description { get; set; }

            public bool IsSystemVat { get; set; }
        }
    }
}

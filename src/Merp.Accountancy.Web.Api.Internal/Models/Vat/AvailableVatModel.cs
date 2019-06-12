using System;

namespace Merp.Accountancy.Web.Api.Internal.Models.Vat
{
    public class AvailableVatModel
    {
        public Guid Id { get; set; }

        public decimal Rate { get; set; }

        public string Description { get; set; }

        public bool AppliedForMinimumTaxPayer { get; set; }
    }
}

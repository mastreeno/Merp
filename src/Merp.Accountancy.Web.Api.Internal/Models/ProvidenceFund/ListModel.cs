using System;

namespace Merp.Accountancy.Web.Api.Internal.Models.ProvidenceFund
{
    public class ListModel
    {
        public Guid Id { get; set; }

        public decimal Rate { get; set; }

        public string Description { get; set; }

        public bool AppliedInWithholdingTax { get; set; }
    }
}

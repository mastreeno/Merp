using System;

namespace Merp.Accountancy.Web.Api.Internal.Models.Settings
{
    public class DefaultsModel
    {
        public bool MinimumTaxpayerRegime { get; set; }

        public bool ElectronicInvoiceEnabled { get; set; }

        public bool SplitPayment { get; set; }

        public VatRateDescriptor VatRate { get; set; }

        public ProvidenceFundDescriptor ProvidenceFund { get; set; }

        public WithholdingTaxDescriptor WithholdingTax { get; set; }

        public class VatRateDescriptor
        {
            public Guid Id { get; set; }

            public decimal Rate { get; set; }
        }

        public class ProvidenceFundDescriptor
        {
            public Guid Id { get; set; }
            
            public string Description { get; set; }

            public decimal Rate { get; set; }

            public bool AppliedInWithholdingTax { get; set; }
        }

        public class WithholdingTaxDescriptor
        {
            public Guid Id { get; set; }

            public string Description { get; set; }

            public decimal Rate { get; set; }

            public decimal TaxableAmountRate { get; set; }
        }
    }
}

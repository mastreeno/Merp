namespace Merp.Accountancy.Web.Models
{
    public class WithholdingTaxModel
    {
        public string Description { get; set; }

        public decimal Rate { get; set; }

        public decimal TaxableAmountRate { get; set; }

        public decimal Amount { get; set; }
    }
}

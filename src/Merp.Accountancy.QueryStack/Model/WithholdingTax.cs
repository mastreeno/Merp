namespace Merp.Accountancy.QueryStack.Model
{
    public class WithholdingTax
    {
        public string Description { get; set; }

        public decimal Rate { get; set; }

        public decimal TaxableAmountRate { get; set; }

        public decimal Amount { get; set; }
    }
}

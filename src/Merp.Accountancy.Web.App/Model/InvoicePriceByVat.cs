namespace Merp.Accountancy.Web.App.Model
{
    public class InvoicePriceByVat
    {
        public decimal TaxableAmount { get; set; }

        public decimal VatRate { get; set; }

        public decimal VatAmount { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal? ProvidenceFundAmount { get; set; }
    }
}

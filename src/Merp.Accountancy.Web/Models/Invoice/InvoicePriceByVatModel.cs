namespace Merp.Accountancy.Web.Models.Invoice
{
    public class InvoicePriceByVatModel
    {
        public decimal TaxableAmount { get; set; }

        public decimal VatRate { get; set; }

        public decimal VatAmount { get; set; }

        public decimal TotalPrice { get; set; }
    }
}

namespace Merp.Accountancy.Web.Models.Draft
{
    public class DraftPriceByVatModel
    {
        public int Id { get; set; }

        public decimal TaxableAmount { get; set; }

        public decimal VatRate { get; set; }

        public decimal VatAmount { get; set; }

        public decimal TotalPrice { get; set; }
    }
}

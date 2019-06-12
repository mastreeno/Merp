using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Drafts.Model
{
    public class PriceByVat
    {
        [Key]
        public int Id { get; set; }

        public decimal TaxableAmount { get; set; }

        public decimal VatRate { get; set; }

        public decimal VatAmount { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal? ProvidenceFundAmount { get; set; }
    }
}

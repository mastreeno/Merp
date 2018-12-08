using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Drafts.Model
{
    public class DraftLineItem
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal Vat { get; set; }
    }
}

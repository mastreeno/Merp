using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models.Draft
{
    public class DraftLineItemModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal Vat { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models.Invoice
{
    public class InvoiceLineItemModel
    {
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal Vat { get; set; }
    }
}

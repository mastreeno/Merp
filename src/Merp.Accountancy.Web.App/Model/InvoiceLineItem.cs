using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.App.Model
{
    public class InvoiceLineItem
    {
        public string? Code { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal Vat { get; set; }

        public string VatDescription { get; set; } = string.Empty;
    }
}

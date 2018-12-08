using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models.Invoice
{
    public class NonTaxableItemModel
    {
        [Required]
        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
}

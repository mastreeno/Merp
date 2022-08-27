using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.App.Model
{
    public class NonTaxableItem
    {
        [Required]
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}

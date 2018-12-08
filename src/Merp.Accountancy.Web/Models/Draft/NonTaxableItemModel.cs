using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models.Draft
{
    public class NonTaxableItemModel
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
}

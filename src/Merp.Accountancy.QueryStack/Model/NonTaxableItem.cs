using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.QueryStack.Model
{
    public class NonTaxableItem
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
}

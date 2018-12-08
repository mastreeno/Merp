using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models
{
    public class PositiveMoney
    {
        [Range(0, int.MaxValue)]
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }
    }
}

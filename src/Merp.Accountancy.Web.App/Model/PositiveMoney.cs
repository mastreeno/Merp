using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.App.Model
{
    public record PositiveMoney
    {
        [Range(0, int.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Api.Internal.Models.Vat
{
    public class CreateModel
    {
        [Required]
        public decimal Rate { get; set; }

        [Required]
        public string Description { get; set; }
    }
}

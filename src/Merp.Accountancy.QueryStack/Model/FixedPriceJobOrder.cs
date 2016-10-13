using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Model
{
    public class FixedPriceJobOrder : JobOrder
    {
        [Required]
        public decimal Price { get; set; }
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
    }
}

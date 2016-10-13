using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Model
{
    public class TimeAndMaterialJobOrder : JobOrder
    {
        public decimal Value { get; set; }
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }
        public DateTime? DateOfExpiration { get; set; }

    }
}

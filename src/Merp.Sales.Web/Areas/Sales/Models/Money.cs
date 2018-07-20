using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Sales.Web.Areas.Sales.Models
{
    public class Money
    {
        [Range(0, int.MaxValue)]
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [StringLength(3, MinimumLength=3)]
        public string Currency { get; set; }
    }
}
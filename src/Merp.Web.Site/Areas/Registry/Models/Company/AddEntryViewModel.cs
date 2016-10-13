using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class AddEntryViewModel
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string VatIndex { get; set; }
    }
}
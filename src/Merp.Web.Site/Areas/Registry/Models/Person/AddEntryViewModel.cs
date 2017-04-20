using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Registry.Models.Person
{
    public class AddEntryViewModel
    {
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("National Identification Number")]
        [Required]
        public string NationalIdentificationNumber { get; set; }

        [DisplayName("VAT number")]
        public string VatNumber { get; set; }

        public PostalAddress Address { get; set; }
    }
}
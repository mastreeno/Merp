using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Models.Company
{
    public class InfoViewModel
    {
        public Guid CompanyUid { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [DisplayName("Vat Number")]
        public string VatNumber { get; set; }
        [DisplayName("National Identification Number")]
        public string NationalIdentificationNumber { get; set; }
        [DisplayName("Legal Address")]
        public PostalAddress LegalAddress { get; set; }
    }
}

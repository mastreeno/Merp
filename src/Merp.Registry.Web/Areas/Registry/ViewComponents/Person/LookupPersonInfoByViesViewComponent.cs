using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Merp.Web.Site.Areas.Registry.ViewComponents.Person
{
    public class LookupPersonInfoByViesViewComponent : ViewComponent
    {
        private static IEnumerable<ViewModel.Country> Countries;

        static LookupPersonInfoByViesViewComponent()
        {
            var countries = new List<ViewModel.Country>
            {
                new ViewModel.Country { Code = "AT", DisplayName = "AT-Austria" },
                new ViewModel.Country { Code = "BE", DisplayName = "BE-Belgio" },
                new ViewModel.Country { Code = "BG", DisplayName = "BG-Bulgaria" },
                new ViewModel.Country { Code = "CY", DisplayName = "CY-Cipro" },
                new ViewModel.Country { Code = "CZ", DisplayName = "CZ-Repubblica ceca" },
                new ViewModel.Country { Code = "DE", DisplayName = "DE-Germania" },
                new ViewModel.Country { Code = "DK", DisplayName = "DK-Danimarca" },
                new ViewModel.Country { Code = "EE", DisplayName = "EE-Estonia" },
                new ViewModel.Country { Code = "EL", DisplayName = "EL-Grecia" },
                new ViewModel.Country { Code = "ES", DisplayName = "ES-Spagna" },
                new ViewModel.Country { Code = "FI", DisplayName = "FI-Finlandia" },
                new ViewModel.Country { Code = "FR", DisplayName = "FR-Francia" },
                new ViewModel.Country { Code = "GB", DisplayName = "GB-Regno Unito" },
                new ViewModel.Country { Code = "HR", DisplayName = "HR-Croazia" },
                new ViewModel.Country { Code = "HU", DisplayName = "HU-Ungheria" },
                new ViewModel.Country { Code = "IE", DisplayName = "IE-Irlanda" },
                new ViewModel.Country { Code = "IT", DisplayName = "IT-Italia" },
                new ViewModel.Country { Code = "LT", DisplayName = "LT-Lituania" },
                new ViewModel.Country { Code = "LU", DisplayName = "LU-Lussemburgo" },
                new ViewModel.Country { Code = "LV", DisplayName = "LV-Lettonia" },
                new ViewModel.Country { Code = "MT", DisplayName = "MT-Malta" },
                new ViewModel.Country { Code = "NL", DisplayName = "NL-Paesi Bassi" },
                new ViewModel.Country { Code = "PL", DisplayName = "PL-Polonia" },
                new ViewModel.Country { Code = "PT", DisplayName = "PT-Portogallo" },
                new ViewModel.Country { Code = "RO", DisplayName = "RO-Romania" },
                new ViewModel.Country { Code = "SE", DisplayName = "SE-Svezia" },
                new ViewModel.Country { Code = "SI", DisplayName = "SI-Slovenia" },
                new ViewModel.Country { Code = "SK", DisplayName = "SK-Slovacchia" }
            };
            Countries = countries;
        }

        public IViewComponentResult Invoke(string id = "lookup_person_info_by_vies")
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = id;
            var model = new ViewModel
            {
                Countries = Countries,
                CountryCode = "IT"
            };
            return View(model);
        }

        public class ViewModel
        {
            public IEnumerable<Country> Countries { get; set; }

            [Required]
            [DisplayName("Country")]
            public string CountryCode { get; set; }

            [Required]
            [DisplayName("Vat Number")]
            public string VatNumber { get; set; }

            public class Country
            {
                public string Code { get; set; }

                public string DisplayName { get; set; }
            }
        }
    }
}

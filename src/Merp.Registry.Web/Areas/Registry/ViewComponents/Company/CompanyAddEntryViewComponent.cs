using Merp.Web.Site.Areas.Registry.Models.Company;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.ViewComponents.Company
{
    public class CompanyAddEntryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AddEntryViewModel model, string mode = "default", string fieldPrefix = "")
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = fieldPrefix;
            return View(mode, model ?? new AddEntryViewModel());
        }
    }
}

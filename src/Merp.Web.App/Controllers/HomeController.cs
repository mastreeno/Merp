using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Merp.Web.App.Mvc;
using Merp.Web.App.Services.Url;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Web.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly UrlBuilder _urlBuilder;

        public HomeController(UrlBuilder urlBuilder)
        {
            _urlBuilder = urlBuilder ?? throw new ArgumentNullException(nameof(urlBuilder));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetJsEndpoints()
        {
            return new JavascriptResult(_urlBuilder.ToJavascriptObject());
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}

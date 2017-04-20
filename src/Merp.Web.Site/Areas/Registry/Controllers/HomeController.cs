using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Merp.Web.Site.Areas.Registry.Controllers
{
    [Area("Registry")]
    [Authorize(Roles = "Registry")]
    public class HomeController : Controller
    {
        // GET: Registry/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
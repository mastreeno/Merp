using Merp.Web.Site.Areas.Accountancy.Models.Invoice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Merp.Web.Site.Areas.Accountancy.Controllers
{
    [Area("Accountancy")]
    public class HomeController : Controller
    {
        // GET: Accountancy/Home
        public ActionResult Index()
        {
            return View();
        }


    }
}
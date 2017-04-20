using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Merp.Web.Site.Areas.Accountancy.Controllers
{
    [Area("Accountancy")]
    [Authorize(Roles = "Accountancy")]
    public class HomeController : Controller
    {
        // GET: Accountancy/Home
        public ActionResult Index()
        {
            return View();
        }


    }
}
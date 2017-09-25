using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.Accountancy.WorkerServices;
using System;

namespace Merp.Web.Site.Areas.Accountancy.Controllers
{
    [Area("Accountancy")]
    [Authorize(Roles = "Accountancy")]
    public class HomeController : Controller
    {
        public HomeControllerWorkerServices WorkerServices { get; private set; }

        public HomeController(HomeControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        // GET: Accountancy/Home
        public ActionResult Index()
        {
            var model = WorkerServices.GetIndexViewModel();
            return View(model);
        }


    }
}
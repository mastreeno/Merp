using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.Registry.Models.Company;
using Merp.Web.Site.Areas.Registry.WorkerServices;

namespace Merp.Web.Site.Areas.Registry.Controllers
{
    [Area("Registry")]
    [Authorize(Roles = "Registry")]
    public class CompanyController : Controller
    {
        public CompanyControllerWorkerServices WorkerServices { get; private set; }

        public CompanyController(CompanyControllerWorkerServices workerServices)
        {
            if(workerServices==null)
                throw new ArgumentNullException(nameof(workerServices));

            WorkerServices = workerServices;
        }

        [HttpGet]
        public ActionResult AddEntry()
        {
            var model = new AddEntryViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddEntry(AddEntryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.AddEntry(model);
            return Redirect("/Registry/");
        }

        [HttpGet]
        public ActionResult Info(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetInfoViewModel(id.Value);
            if(model==null)
                return NotFound();
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangeName(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetChangeNameViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeName(ChangeNameViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);
            WorkerServices.PostChangeNameViewModel(model);
            return Redirect("/Registry/");
        }
    }
}
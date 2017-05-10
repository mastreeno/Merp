using Merp.Web.Site.Areas.Registry.Models.Person;
using Merp.Web.Site.Areas.Registry.WorkerServices;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.Controllers
{
    [Area("Registry")]
    [Authorize(Roles = "Registry")]
    public class PersonController : Controller
    {
        public PersonControllerWorkerServices WorkerServices { get; private set; }

        public PersonController(PersonControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult AddEntry(bool ajax = false, string fieldPrefix = "")
        {
            var model = WorkerServices.GetAddEntryViewModel();

            if (ajax)
            {
                return ViewComponent("AddEntry", new { model, mode = "modal", fieldPrefix });
            }
                
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEntry(AddEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.AddEntry(model);
            return Redirect("/Registry/");
        }

        [HttpPut]
        public  IActionResult AddEntry(AddEntryViewModel model, string fieldPrefix = "")
        {
            if (!ModelState.IsValid)
            {
                return ViewComponent("AddEntry", new { model, mode = "default", fieldPrefix });
            }

            WorkerServices.AddEntry(model);
                        
            ModelState.Clear();

            model = WorkerServices.GetAddEntryViewModel();
            return ViewComponent("AddEntry", new { model, mode = "default", fieldPrefix });
        }

        [HttpGet]
        public IActionResult Info(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetInfoViewModel(id.Value);
            return View(model);
        }


        [HttpGet]
        public ActionResult ChangeAddress(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetChangeAddressViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeAddress(ChangeAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.ChangeAddress(model);
            return Redirect("/Registry/");
        }

        [HttpGet]
        public ActionResult ChangeContactInfo(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetChangeContactInfoViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeContactInfo(ChangeContactInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.ChangeContactInfo(model);
            return Redirect("/Registry/");
        }
    }
}
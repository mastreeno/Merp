using Merp.Web.Site.Areas.Registry.Models.Person;
using Merp.Web.Site.Areas.Registry.WorkerServices;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Merp.Web.Site.Areas.Registry.Controllers
{
    [Area("Registry")]
    [Authorize(Roles = "Registry")]
    public class PersonController : Controller
    {
        public PersonControllerWorkerServices WorkerServices { get; private set; }

        public PersonController(PersonControllerWorkerServices workerServices)
        {
            if(workerServices==null)
                throw new ArgumentNullException(nameof(workerServices));

            WorkerServices = workerServices;
        }

        [HttpGet]
        public IActionResult AddEntry()
        {
            var model = new AddEntryViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEntry(AddEntryViewModel model)
        {
            if(!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.AddEntry(model);
            return Redirect("/Registry/");
        }

        [HttpGet]
        public IActionResult Info(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetInfoViewModel(id.Value);
            return View(model);
        }
    }
}
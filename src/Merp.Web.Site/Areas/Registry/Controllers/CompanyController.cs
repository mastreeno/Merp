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
            WorkerServices.ChangeName(model);
            return Redirect("/Registry/");
        }

        [HttpGet]
        public ActionResult ChangeLegalAddress(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetChangeLegalAddressViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeLegalAddress(ChangeLegalAddressViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.ChangeLegalAddress(model);
            return Redirect("/Registry/");
        }

        [HttpGet]
        public ActionResult ChangeShippingAddress(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetChangeShippingAddressViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeShippingAddress(ChangeShippingAddressViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.ChangeShippingAddress(model);
            return Redirect("/Registry/");
        }
        
        [HttpGet]
        public ActionResult ChangeBillingAddress(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetChangeBillingAddressViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeBillingAddress(ChangeBillingAddressViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.ChangeBillingAddress(model);
            return Redirect("/Registry/");
        }

        [HttpGet]
        public ActionResult AssociateAdministrativeContact(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetAssociateAdministrativeContactViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult AssociateAdministrativeContact(AssociateAdministrativeContactViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.AssociateAdministrativeContact(model);
            return Redirect("/Registry/");
        }

        [HttpGet]
        public ActionResult AssociateMainContact(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();
            var model = WorkerServices.GetAssociateMainContactViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult AssociateMainContact(AssociateMainContactViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.AssociateMainContact(model);
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
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.ChangeContactInfo(model);
            return Redirect("/Registry/");
        }


        

    }
}
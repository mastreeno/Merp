using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.Registry.Models.Company;
using Merp.Web.Site.Areas.Registry.WorkerServices;
using Merp.Web.Site.Areas.Registry.ViewComponents.Company;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Merp.Web.Site.Areas.Registry.Controllers
{
    [Area("Registry")]
    [Authorize(Roles = "Registry")]
    public class CompanyController : Controller
    {
        public CompanyControllerWorkerServices WorkerServices { get; private set; }

        public CompanyController(CompanyControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public ActionResult AddEntry(bool ajax = false, string fieldPrefix = "")
        {
            var model = WorkerServices.GetAddEntryViewModel();

            if (ajax)
            {
                return ViewComponent(typeof(CompanyAddEntryViewComponent), new { model, mode = "modal", fieldPrefix });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddEntry(AddEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.AddEntry(model);
            return RedirectToRoute("registry", new { });
        }

        [HttpPut]
        public IActionResult AddEntry(AddEntryViewModel model, string fieldPrefix = "")
        {
            if (!ModelState.IsValid)
            {
                return Json(new SerializableError(ModelState));
            }
            WorkerServices.AddEntry(model);
            return Ok();
        }

        [HttpGet]
        public IActionResult LookupCompanyInfoByVies(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            return ViewComponent(typeof(LookupCompanyInfoByViesViewComponent), new { id });
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
            ValidateAgainstPersistence(model);
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            WorkerServices.ChangeName(model);
            return Ok();
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
            ValidateAgainstPersistence(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            WorkerServices.ChangeLegalAddress(model);
            return Ok(); ;
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
            ValidateAgainstPersistence(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            WorkerServices.ChangeShippingAddress(model);
            return Ok();
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
            ValidateAgainstPersistence(model);
            if (!ModelState.IsValid)
            {
                var rehydratedModel = WorkerServices.GetChangeBillingAddressViewModel(model);
                return View(rehydratedModel);
            }
            WorkerServices.ChangeBillingAddress(model);
            return RedirectToRoute("registry", new { });
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
            return RedirectToRoute("registry", new { });
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
            return RedirectToRoute("registry", new { });
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
                return BadRequest(ModelState);
            }
            WorkerServices.ChangeContactInfo(model);
            return Ok();
        }

        #region Helper Methods        

        private void ValidateAgainstPersistence(ChangeLegalAddressViewModel model)
        {
            var companyDto = WorkerServices.GetChangeLegalAddressViewModelCompanyDto(model.CompanyId);
            var persistenceValidationModelState = model.Validate(companyDto);
            ModelState.Merge(persistenceValidationModelState);
        }

        private void ValidateAgainstPersistence(ChangeBillingAddressViewModel model)
        {
            var companyDto = WorkerServices.GetChangeBillingAddressViewModelCompanyDto(model.CompanyId);
            var persistenceValidationModelState = model.Validate(companyDto);
            ModelState.Merge(persistenceValidationModelState);
        }

        private void ValidateAgainstPersistence(ChangeShippingAddressViewModel model)
        {
            var companyDto = WorkerServices.GetChangeShippingAddressViewModelCompanyDto(model.CompanyId);
            var persistenceValidationModelState = model.Validate(companyDto);
            ModelState.Merge(persistenceValidationModelState);
        }

        private void ValidateAgainstPersistence(ChangeNameViewModel model)
        {
            var companyDto = WorkerServices.GetChangeNameViewModelCompanyDto(model.CompanyId);
            var persistenceValidationModelState = model.Validate(companyDto);
            ModelState.Merge(persistenceValidationModelState);
        }

        #endregion
    }
}
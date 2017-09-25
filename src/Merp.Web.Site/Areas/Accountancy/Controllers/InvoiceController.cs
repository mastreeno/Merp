using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.Accountancy.Models.Invoice;
using Merp.Web.Site.Areas.Accountancy.WorkerServices;

namespace Merp.Web.Site.Areas.Accountancy.Controllers
{
    [Area("Accountancy")]
    [Authorize(Roles = "Accountancy")]
    public class InvoiceController : Controller
    {
        public InvoiceControllerWorkerServices WorkerServices { get; private set; }

        public InvoiceController(InvoiceControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public ActionResult Issue()
        {
            var model = WorkerServices.GetIssueViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Issue(IssueViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.Issue(model);
            return Redirect("/Accountancy/");
        }

        [HttpGet]
        public ActionResult Register()
        {
            var model = WorkerServices.GetRegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.Register(model);
            return Redirect("/Accountancy/");
        }

        #region LinkIncomingInvoiceToJobOrder
        [HttpGet]
        public ActionResult ListOfIncomingInvoicesNotLinkedToAJobOrder()
        {
            var model = WorkerServices.GetListOfIncomingInvoicesNotLinkedToAJobOrderViewModel();
            return Json(model);
        }
        [HttpGet]
        public ActionResult IncomingInvoicesNotLinkedToAJobOrder()
        {
            var model = new IncomingInvoicesNotLinkedToAJobOrderViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult LinkIncomingInvoiceToJobOrder(Guid? id)
        {
            var model = WorkerServices.GetLinkIncomingInvoiceToJobOrderViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult LinkIncomingInvoiceToJobOrder(LinkIncomingInvoiceToJobOrderViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.LinkIncomingInvoiceToJobOrder(model);
            return Redirect("/Accountancy/");
        }
        #endregion

        #region LinkOutgoingInvoiceToJobOrder
        [HttpGet]
        public ActionResult ListOfOutgoingInvoicesNotLinkedToAJobOrder()
        {
            var model = WorkerServices.GetListOfOutgoingInvoicesNotLinkedToAJobOrderViewModel();
            return Json(model);
        }
        [HttpGet]
        public ActionResult OutgoingInvoicesNotLinkedToAJobOrder()
        {
            var model = new OutgoingInvoicesNotLinkedToAJobOrderViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult LinkOutgoingInvoiceToJobOrder(Guid? id)
        {
            var model = WorkerServices.GetLinkOutgoingInvoiceToJobOrderViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult LinkOutgoingInvoiceToJobOrder(LinkOutgoingInvoiceToJobOrderViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.LinkOutgoingInvoiceToJobOrder(model);
            return Redirect("/Accountancy/");
        }
        #endregion
    }
}
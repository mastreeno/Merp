using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.Accountancy.Models.JobOrder;
using Merp.Web.Site.Areas.Accountancy.WorkerServices;
using System.Collections.Generic;

namespace Merp.Web.Site.Areas.Accountancy.Controllers
{
    [Area("Accountancy")]
    [Authorize(Roles = "Accountancy")]
    public class JobOrderController : Controller
    {
        public JobOrderControllerWorkerServices WorkerServices { get; private set; }

        public JobOrderController(JobOrderControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        // GET: JobOrder
        [HttpGet]
        public ActionResult Index()
        {
            var model = WorkerServices.GetIndexViewModel();
            return View(model);
        }

        [HttpGet]
        public IEnumerable<IndexViewModel.JobOrder> GetList(bool? currentOnly, Guid? customerId, string jobOrderName)
        {
            var model = WorkerServices.GetList(currentOnly.HasValue ? currentOnly.Value : false, 
                                            customerId, 
                                            jobOrderName);
            return model;
        }

        [HttpGet]
        public ActionResult Detail(Guid? id)
        {
            return Redirect(string.Format("/Accountancy/JobOrder/JobOrderDetail/{0}", id));
        }

        [HttpGet]
        public ActionResult GetBalance(Guid? jobOrderId, DateTime? dateFrom, DateTime? dateTo, BalanceViewModel.Scale scale)
        {
            if(!jobOrderId.HasValue || !dateFrom.HasValue || !dateTo.HasValue)
                return BadRequest();

            var model = WorkerServices.GetBalanceViewModel(jobOrderId.Value, dateFrom.Value, dateTo.Value, scale);
            return Merp.Web.Mvc.JsonNetResult.JsonNet(model);
        }

        public ActionResult IncomingInvoicesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = WorkerServices.GetIncomingInvoicesAssociatedToJobOrderViewModel(jobOrderId);
            return View(model);
        }

        public ActionResult OutgoingInvoicesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = WorkerServices.GetOutgoingInvoicesAssociatedToJobOrderViewModel(jobOrderId);
            return View(model);
        }

        #region Job Orders
        [HttpGet]
        public ActionResult CreateJobOrder()
        {
            var model = WorkerServices.GetCreateJobOrderViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateJobOrder(CreateJobOrderViewModel model)
        {
            if(!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.CreateJobOrder(model);
            return Redirect("/Accountancy/JobOrder");
        }

        public ActionResult JobOrderDetail(Guid? id)
        {
            var model = WorkerServices.GetJobOrderDetailViewModel(id.Value);
            return View(model);
        }

        [HttpGet]
        public ActionResult ExtendJobOrder(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var model = WorkerServices.GetExtendJobOrderViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult ExtendJobOrder(ExtendJobOrderViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.ExtendJobOrder(model);
            return Redirect("/Accountancy/JobOrder");
        }

        [HttpGet]
        public ActionResult MarkJobOrderAsCompleted(Guid? id)
        {
            var model = WorkerServices.GetMarkJobOrderAsCompletedViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult MarkJobOrderAsCompleted(MarkJobOrderAsCompletedViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.MarkJobOrderAsCompleted(model);
            return Redirect("/Accountancy/JobOrder");
        }

        [HttpGet]
        public decimal EvaluateJobOrderBalance(Guid id)
        {
            var model = WorkerServices.GetEvaluateJobOrderBalance(id);
            return model;
        }
        #endregion
        
    }
}
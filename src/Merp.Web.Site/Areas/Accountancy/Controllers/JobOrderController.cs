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
            switch (WorkerServices.GetDetailViewModel(id.Value))
            {
                case "FixedPrice":
                    return Redirect(string.Format("/Accountancy/JobOrder/FixedPriceJobOrderDetail/{0}", id));
                case "TimeAndMaterial":
                    return Redirect(string.Format("/Accountancy/JobOrder/TimeAndMaterialJobOrderDetail/{0}", id));
                default:
                    return RedirectToAction("Index");
            }
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

        #region Fixed Price Job Orders
        [HttpGet]
        public ActionResult CreateFixedPrice()
        {
            var model = WorkerServices.GetCreateFixedPriceViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateFixedPrice(CreateFixedPriceViewModel model)
        {
            if(!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.CreateFixedPriceJobOrder(model);
            return Redirect("/Accountancy/JobOrder");
        }

        public ActionResult FixedPriceJobOrderDetail(Guid? id)
        {
            var model = WorkerServices.GetFixedPriceJobOrderDetailViewModel(id.Value);
            return View(model);
        }

        [HttpGet]
        public ActionResult ExtendFixedPrice(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var model = WorkerServices.GetExtendFixedPriceViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult ExtendFixedPrice(ExtendFixedPriceViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.ExtendFixedPriceJobOrder(model);
            return Redirect("/Accountancy/JobOrder");
        }

        [HttpGet]
        public ActionResult MarkFixedPriceJobOrderAsCompleted(Guid? id)
        {
            var model = WorkerServices.GetMarkFixedPriceJobOrderAsCompletedViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult MarkFixedPriceJobOrderAsCompleted(MarkFixedPriceJobOrderAsCompletedViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.MarkFixedPriceJobOrderAsCompleted(model);
            return Redirect("/Accountancy/JobOrder");
        }

        [HttpGet]
        public decimal EvaluateFixedPriceJobOrderBalance(Guid id)
        {
            var model = WorkerServices.GetEvaluateFixedPriceJobOrderBalance(id);
            return model;
        }
        #endregion

        #region Time And Material Job Orders
        [HttpGet]
        public ActionResult CreateTimeAndMaterial()
        {
            var model = WorkerServices.GetCreateTimeAndMaterialViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateTimeAndMaterial(CreateTimeAndMaterialViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.CreateTimeAndMaterialJobOrder(model);
            return Redirect("/Accountancy/JobOrder");
        }

        public ActionResult TimeAndMaterialJobOrderDetail(Guid? id)
        {
            var model = WorkerServices.GetTimeAndMaterialJobOrderDetailViewModel(id.Value);
            return View(model);
        }

        [HttpGet]
        public ActionResult ExtendTimeAndMaterial(Guid? id)
        {
            var model = WorkerServices.GetExtendTimeAndMaterialViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult ExtendTimeAndMaterial(ExtendTimeAndMaterialViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.ExtendTimeAndMaterialJobOrder(model);
            return Redirect("/Accountancy/JobOrder");
        }

        [HttpGet]
        public ActionResult MarkTimeAndMaterialJobOrderAsCompleted(Guid? id)
        {
            var model = WorkerServices.GetMarkTimeAndMaterialJobOrderAsCompletedViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult MarkTimeAndMaterialJobOrderAsCompleted(MarkTimeAndMaterialJobOrderAsCompletedViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            WorkerServices.MarkTimeAndMaterialJobOrderAsCompleted(model);
            return Redirect("/Accountancy/JobOrder");
        }

        [HttpGet]
        public decimal EvaluateTimeAndMaterialJobOrderBalance(Guid id)
        {
            var model = WorkerServices.GetEvaluateTimeAndMaterialJobOrderBalance(id);
            return model;
        }
        #endregion
        
    }
}
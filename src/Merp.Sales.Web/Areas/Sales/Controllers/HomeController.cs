using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.Sales.Models.Home;
using System.Collections.Generic;
using Merp.Sales.Web.Areas.Sales.WorkerServices;

namespace Merp.Sales.Web.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Authorize(Roles = "Sales")]
    public class HomeController : Controller
    {
        public HomeControllerWorkerServices WorkerServices { get; private set; }

        public HomeController(HomeControllerWorkerServices workerServices)
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
        public IEnumerable<IndexViewModel.Project> GetList(bool? currentOnly, Guid? customerId, string jobOrderName)
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

        #region Job Orders
        [HttpGet]
        public ActionResult CreateJobOrder()
        {
            var model = WorkerServices.GetRegisterProjectViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateJobOrder(RegisterProjectViewModel model)
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
            var model = WorkerServices.GetProjectDetailViewModel(id.Value);
            return View(model);
        }

        [HttpGet]
        public ActionResult MarkJobOrderAsCompleted(Guid? id)
        {
            var model = WorkerServices.GetMarkJobOrderAsCompletedViewModel(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult MarkJobOrderAsCompleted(MarkProjectAsCompletedViewModel model)
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
            var model = WorkerServices.GetEvaluateProjectBalance(id);
            return model;
        }
        #endregion
        
    }
}
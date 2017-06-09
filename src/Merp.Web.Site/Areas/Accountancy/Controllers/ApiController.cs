using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.Accountancy.WorkerServices;
using Merp.Web.Site.Areas.Accountancy.Models.Api;

namespace Merp.Web.Site.Areas.Accountancy.Controllers
{
    [Area("Accountancy")]
    [Authorize]
    public class ApiController : Controller
    {
        public JobOrderControllerWorkerServices WorkerServices { get; private set; }

        public ApiController(JobOrderControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IEnumerable<JobOrderListViewModel.JobOrder> GetList(bool? currentOnly, Guid? customerId, string jobOrderName)
        {
            var list = WorkerServices.GetList(currentOnly.HasValue ? currentOnly.Value : false,
                                            customerId,
                                            jobOrderName);

            var model = list.Select(jo => new JobOrderListViewModel.JobOrder()
            {
                CustomerId = jo.CustomerId,
                CustomerName = jo.CustomerName,
                Id = jo.Id,
                IsCompleted = jo.IsCompleted,
                Name = jo.Name,
                Number = jo.Number,
                OriginalId = jo.OriginalId,
                Tenant = "Tenant1"
            });
            return model;
        }
    }
}
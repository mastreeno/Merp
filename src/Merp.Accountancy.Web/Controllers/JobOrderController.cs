using System;
using System.Threading.Tasks;
using Merp.Accountancy.Web.Models.JobOrder;
using Merp.Accountancy.Web.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Accountancy.Web.Controllers
{
    [Authorize]
    public class JobOrderController : ControllerBase
    {
        public JobOrderControllerWorkerServices WorkerServices { get; private set; }

        public JobOrderController(JobOrderControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult Search(bool? currentOnly, Guid? customerId, string jobOrderName, int page = 1, int size = 20)
        {
            var viewModel = WorkerServices.SearchJobOrders(currentOnly ?? false, customerId, jobOrderName, page, size);
            return Ok(viewModel);
        }

        [HttpGet]
        public IActionResult GetJobOrderCustomers()
        {
            var viewModel = WorkerServices.GetJobOrderCustomers();
            return Ok(viewModel);
        }

        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            var viewModel = WorkerServices.GetJobOrderDetail(id);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.CreateJobOrderAsync(model);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetBalance(Guid? jobOrderId, DateTime? dateFrom, DateTime? dateTo, BalanceModel.Scale scale)
        {
            if (!jobOrderId.HasValue || !dateFrom.HasValue || !dateTo.HasValue)
                return BadRequest();

            var model = WorkerServices.GetBalanceViewModel(jobOrderId.Value, dateFrom.Value, dateTo.Value, scale);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> Extend(Guid id, [FromBody]ExtendModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ExtendJobOrderAsync(id, model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> MarkAsCompleted(Guid id, [FromBody]MarkAsCompletedModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.MarkJobOrderAsCompletedAsync(id, model);
            return Ok();
        }

        [HttpGet]
        public IActionResult EvaluateBalance(Guid id)
        {
            var balance = WorkerServices.GetEvaluateJobOrderBalance(id);
            return Ok(balance);
        }

        [HttpGet]
        public IActionResult GetOutgoingCreditNotesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = WorkerServices.GetOutgoingCreditNotesAssociatedToJobOrder(jobOrderId);
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetOutgoingInvoicesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = WorkerServices.GetOutgoingInvoicesAssociatedToJobOrder(jobOrderId);
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetIncomingCreditNotesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = WorkerServices.GetIncomingCreditNotesAssociatedToJobOrder(jobOrderId);
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetIncomingInvoicesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = WorkerServices.GetIncomingInvoicesAssociatedToJobOrder(jobOrderId);
            return Ok(model);
        }
    }
}
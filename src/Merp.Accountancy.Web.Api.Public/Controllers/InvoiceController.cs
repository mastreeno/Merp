using Merp.Accountancy.Web.Api.Public.Models;
using Merp.Accountancy.Web.Api.Public.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public.Controllers
{
    //[Authorize]
    public class InvoiceController : ControllerBase
    {
        public InvoiceControllerWorkerServices WorkerServices { get; private set; }

        public InvoiceController(InvoiceControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        #region Incoming invoices
        [HttpPost]
        public async Task<IActionResult> ImportIncoming([FromBody]ImportIncomingInvoiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ImportIncomingInvoiceAsync(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LinkIncomingInvoiceToJobOrder([FromBody]LinkIncomingInvoiceToJobOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.LinkIncomingInvoiceToJobOrderAsync(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkIncomingInvoiceAsPaid([FromBody]MarkIncomingInvoiceAsPaidModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.MarkIncomingInvoiceAsPaid(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkIncomingInvoiceAsOverdue([FromBody]MarkIncomingInvoiceAsOverdueModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.MarkIncomingInvoiceAsOverdue(model);

            return Ok();
        }
        #endregion

        #region Outgoing invoices
        [HttpPost]
        public async Task<IActionResult> ImportOutgoing([FromBody]ImportOutgoingInvoiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ImportOutgoingInvoiceAsync(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LinkOutgoingInvoiceToJobOrder([FromBody]LinkOutgoingInvoiceToJobOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.LinkOutgoingInvoiceToJobOrderAsync(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkOutgoingInvoiceAsPaid([FromBody]MarkOutgoingInvoiceAsPaidModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.MarkOutgoingInvoiceAsPaid(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkOutgoingInvoiceAsOverdue([FromBody]MarkOutgoingInvoiceAsOverdueModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.MarkOutgoingInvoiceAsOverdue(model);

            return Ok();
        }
        #endregion
    }
}

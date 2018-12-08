using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Merp.Accountancy.Web.Models.Draft;
using Merp.Accountancy.Web.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Accountancy.Web.Controllers
{
    [Authorize]
    public class DraftController : ControllerBase
    {
        public DraftControllerWorkerServices WorkerServices { get; private set; }

        public DraftController(DraftControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult Search(SearchModel.DraftKind kind, Guid? customerId, DateTime? dateFrom, DateTime? dateTo, int page = 1, int size = 20)
        {
            var model = WorkerServices.SearchDrafts(kind, customerId, dateFrom, dateTo, page, size);
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetDraftCustomers()
        {
            var model = WorkerServices.GetDraftCustomers();
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOutgoingDraft([FromBody]CreateOutgoingDraftModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.CreateOutgoingDraftAsync(model);
            return Ok();
        }

        #region Outgoing invoice Drafts
        [HttpGet]
        public IActionResult EditOutgoingInvoiceDraft(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var model = WorkerServices.GetEditOutgoingInvoiceDraft(id);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> EditOutgoingInvoiceDraft(Guid id, [FromBody]OutgoingInvoiceDraftModel model)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.EditOutgoingInvoiceDraftAsync(id, model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOutgoingInvoiceDraft(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            await WorkerServices.DeleteOutgoingInvoiceDraftAsync(id);
            return Ok();
        }
        #endregion

        #region Outgoing credit note draft
        [HttpGet]
        public IActionResult EditOutgoingCreditNoteDraft(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var model = WorkerServices.GetEditOutgoingCreditNoteDraft(id);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> EditOutgoingCreditNoteDraft(Guid id, [FromBody]OutgoingCreditNoteDraftModel model)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.EditOutgoingCreditNoteDraftAsync(id, model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOutgoingCreditNoteDraft(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            await WorkerServices.DeleteOutgoingCreditNoteDraftAsync(id);
            return Ok();
        }
        #endregion
    }
}
using System;
using System.Threading.Tasks;
using Merp.Accountancy.Web.Models;
using Merp.Accountancy.Web.Models.Invoice;
using Merp.Accountancy.Web.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Accountancy.Web.Controllers
{
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        public InvoiceControllerWorkerServices WorkerServices { get; private set; }

        public InvoiceController(InvoiceControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult Search(SearchModel.InvoiceKind kind, SearchModel.InvoiceState status, Guid? customerId, Guid? supplierId, DateTime? dateFrom, DateTime? dateTo, int page = 1, int size = 20)
        {
            var model = WorkerServices.SearchInvoices(kind, status, customerId, supplierId, dateFrom, dateTo, page, size);
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetSearchInvoiceKinds()
        {
            var model = Enum.GetNames(typeof(SearchModel.InvoiceKind));
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetSearchInvoiceStates()
        {
            var model = Enum.GetNames(typeof(SearchModel.InvoiceState));
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetInvoiceCustomers()
        {
            var model = WorkerServices.GetInvoiceCustomers();
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetInvoiceSuppliers()
        {
            var model = WorkerServices.GetInvoiceSuppliers();
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetInvoicesStats()
        {
            var model = WorkerServices.GetInvoicesStats();
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetVatList()
        {
            var model = WorkerServices.GetVatList();
            return Ok(model);
        }

        #region Outgoing Invoices
        [HttpGet]
        public IActionResult GetOutgoingDocumentTypes()
        {
            var model = Enum.GetNames(typeof(OutgoingDocumentTypes));
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> IssueOutgoingInvoice([FromBody]IssueOutgoingInvoiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.IssueOutgoingInvoiceAsync(model);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> IssueOutgoingCreditNoteFromDraft([FromBody]IssueOutgoingCreditNoteFromDraftModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.IssueOutgoingCreditNoteFromDraftAsync(model);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> IssueOutgoingInvoiceFromDraft([FromBody]IssueOutgoingInvoiceFromDraftModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.IssueOutgoingInvoiceFromDraftAsync(model);
            return Ok();
        }

        [HttpGet]
        public IActionResult OutgoingInvoicesNotLinkedToAJobOrder(OutgoingDocumentTypes? type, string customer, int page = 1, int size = 20)
        {
            var model = WorkerServices.GetOutgoingInvoicesNotLinkedToAJobOrder(type, customer, page, size);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> LinkOutgoingInvoiceToJobOrder(Guid id, [FromBody]LinkOutgoingInvoiceToJobOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.LinkOutgoingInvoiceToJobOrderAsync(id, model);
            return Ok();
        }

        [HttpGet]
        public IActionResult OutgoingCreditNoteDetails(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var model = WorkerServices.GetOutgoingCreditNoteDetails(id);
            return Ok(model);
        }

        [HttpGet]
        public IActionResult OutgoingInvoiceDetails(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var model = WorkerServices.GetOutgoingInvoiceDetails(id);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOutgoingInvoice([FromBody]RegisterOutgoingInvoiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.RegisterOutgoingInvoiceAsync(model);
            return Ok();
        }
        #endregion

        #region Incoming Invoices
        [HttpGet]
        public IActionResult GetIncomingDocumentTypes()
        {
            var model = Enum.GetNames(typeof(IncomingDocumentTypes));
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterIncomingInvoice([FromBody]RegisterIncomingInvoiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.RegisterIncomingInvoiceAsync(model);
            return Ok();
        }

        [HttpGet]
        public IActionResult IncomingInvoicesNotLinkedToAJobOrder(IncomingDocumentTypes? type, string supplier, int page = 1, int size = 20)
        {
            var model = WorkerServices.GetIncomingInvoicesNotLinkedToAJobOrder(type, supplier, page, size);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> LinkIncomingInvoiceToJobOrder(Guid id, [FromBody]LinkIncomingInvoiceToJobOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.LinkIncomingInvoiceToJobOrderAsync(id, model);
            return Ok();
        }

        [HttpGet]
        public IActionResult IncomingCreditNoteDetails(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var model = WorkerServices.GetIncomingCreditNoteDetails(id);
            return Ok(model);
        }

        [HttpGet]
        public IActionResult IncomingInvoiceDetails(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var model = WorkerServices.GetIncomingInvoiceDetails(id);
            return Ok(model);
        }
        #endregion
    }
}
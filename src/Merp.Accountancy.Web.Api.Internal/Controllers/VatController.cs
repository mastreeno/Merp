using System;
using System.Threading.Tasks;
using Merp.Accountancy.Web.Api.Internal.Models.Vat;
using Merp.Accountancy.Web.Api.Internal.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Accountancy.Web.Api.Internal.Controllers
{
    [Authorize]
    public class VatController : ControllerBase
    {
        public VatControllerWorkerServices WorkerServices { get; private set; }

        public VatController(VatControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult GetList(string filter = null, int page = 1, int size = 20)
        {
            var model = WorkerServices.GetVatList(filter, page, size);
            return Ok(model);
        }

        [HttpGet]
        public IActionResult GetAvailableVats(string query = null)
        {
            var model = WorkerServices.GetAvailableVats(query);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.CreateNewVat(model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, [FromBody]EditModel model)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.EditVat(id, model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Unlist(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            await WorkerServices.UnlistVat(id);
            return Ok();
        }
    }
}
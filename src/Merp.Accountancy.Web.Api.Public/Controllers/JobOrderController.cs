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
    public class JobOrderController : ControllerBase
    {
        public JobOrderControllerWorkerServices WorkerServices { get; private set; }

        public JobOrderController(JobOrderControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpPost]
        public async Task<IActionResult> Extend([FromBody]ExtendJobOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ExtendJobOrderAsync(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Import([FromBody]ImportJobOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ImportJobOrderAsync(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsCompleted([FromBody]MarkJobOrderAsCompletedModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.MarkJobOrderAsCompletedAsync(model);

            return Ok();
        }
    }
}

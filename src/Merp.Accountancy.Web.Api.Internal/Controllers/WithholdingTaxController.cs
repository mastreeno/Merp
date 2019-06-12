using System;
using Merp.Accountancy.Web.Api.Internal.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Accountancy.Web.Api.Internal.Controllers
{
    [Authorize]
    public class WithholdingTaxController : ControllerBase
    {
        public WithholdingTaxControllerWorkerServices WorkerServices { get; private set; }

        public WithholdingTaxController(WithholdingTaxControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult GetList(string query = null)
        {
            var model = WorkerServices.GetWithholdingTaxes(query);
            return Ok(model);
        }
    }
}
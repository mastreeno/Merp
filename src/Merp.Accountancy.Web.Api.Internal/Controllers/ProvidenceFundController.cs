using System;
using Merp.Accountancy.Web.Api.Internal.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Accountancy.Web.Api.Internal.Controllers
{
    [Authorize]
    public class ProvidenceFundController : ControllerBase
    {
        public ProvidenceFundControllerWorkerServices WorkerServices { get; private set; }

        public ProvidenceFundController(ProvidenceFundControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult GetList(string query = null)
        {
            var model = WorkerServices.GetProvidenceFunds(query);
            return Ok(model);
        }
    }
}
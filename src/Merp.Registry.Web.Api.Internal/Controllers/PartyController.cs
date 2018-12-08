using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Merp.Registry.Web.Api.Internal.Models.Party;
using Merp.Registry.Web.Api.Internal.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Registry.Web.Api.Internal.Controllers
{
    [Authorize]
    public class PartyController : ControllerBase
    {
        public PartyControllerWorkerServices WorkerServices { get; private set; }

        public PartyController(PartyControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult GetPartyInfoByPattern(string text)
        {
            var viewModel = WorkerServices.GetPartyInfoByPattern(text);
            return Ok(viewModel);
        }

        [HttpGet]
        public IActionResult GetPartyInfoById(Guid id)
        {
            var viewModel = WorkerServices.GetPartyInfoById(id);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.RegisterPartyAsync(model);
            return Ok();
        }
    }
}
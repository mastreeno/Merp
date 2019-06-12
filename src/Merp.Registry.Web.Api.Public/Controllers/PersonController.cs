using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Merp.Registry.Web.Api.Public.Models;
using Merp.Registry.Web.Api.Public.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Registry.Web.Api.Public.Controllers
{
    //[Authorize]
    public class PersonController : ControllerBase
    {
        public PersonControllerWorkerServices WorkerServices { get; private set; }

        public PersonController(PersonControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpPost]
        public async Task<IActionResult> Import([FromBody]ImportPersonModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ImportPersonAsync(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetContactInfo([FromBody]SetPersonContactInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.SetPersonContactInfoAsync(model);

            return Ok();
        }
    }
}
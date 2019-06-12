using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Merp.Accountancy.Web.Api.Internal.Models.Settings;
using Merp.Accountancy.Web.Api.Internal.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Accountancy.Web.Api.Internal.Controllers
{
    [Authorize]
    public class SettingsController : ControllerBase
    {
        public SettingsControllerWorkerServices WorkerServices { get; private set; }

        public SettingsController(SettingsControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult GetDefaults()
        {
            var model = WorkerServices.GetSettingsDefaults();
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDefaults([FromBody]DefaultsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.SaveSettingsDefaults(model);
            return Ok();
        }
    }
}
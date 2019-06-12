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
    public class CompanyController : ControllerBase
    {
        public CompanyControllerWorkerServices WorkerServices { get; private set; }

        public CompanyController(CompanyControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpPost]
        public async Task<IActionResult> AssociateAdministrativeContact([FromBody]AssociateCompanyAdministrativeContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.AssociateCompanyAdministrativeContact(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AssociateMainContact([FromBody]AssociateCompanyMainContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.AssociateCompanyMainContact(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Import([FromBody]ImportCompanyModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ImportCompanyAsync(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetContactInfo([FromBody]SetCompanyContactInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.SetCompanyContactInfoAsync(model);

            return Ok();
        }
    }
}
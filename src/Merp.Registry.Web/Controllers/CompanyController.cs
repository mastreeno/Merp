using System;
using System.Threading.Tasks;
using Acl.RegistryResolutionServices;
using Merp.Registry.Web.Models.Company;
using Merp.Registry.Web.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Registry.Web.Controllers
{
    [Authorize]
    public class CompanyController : ControllerBase
    {
        public CompanyControllerWorkerServices WorkerServices { get; private set; }

        public Resolver ResolverServiceProxy { get; private set; }

        public CompanyController(CompanyControllerWorkerServices workerServices, Resolver resolverServiceProxy)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
            ResolverServiceProxy = resolverServiceProxy ?? throw new ArgumentNullException(nameof(resolverServiceProxy));
        }

        [HttpPost]
        public async Task<IActionResult> AddEntry([FromBody]AddEntryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.AddEntryAsync(model);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetInfo(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("person id cannot be empty");
            }

            var viewModel = WorkerServices.GetInfoViewModel(id);
            return Ok(viewModel);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeCompanyName(Guid id, [FromBody]ChangeNameModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ChangeNameAsync(id, model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeLegalAddress(Guid id, [FromBody]ChangeLegalAddressModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ChangeLegalAddressAsync(id, model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeShippingAddress(Guid id, [FromBody]ChangeShippingAddressModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ChangeShippingAddressAsync(id, model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeBillingAddress(Guid id, [FromBody]ChangeBillingAddressModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ChangeBillingAddressAsync(id, model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeContactInfo(Guid id, [FromBody]ChangeContactInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.ChangeContactInfoAsync(id, model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> AssociateMainContact(Guid id, [FromBody]AssociateMainContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.AssociateMainContactAsync(id, model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> AssociateAdministrativeContact(Guid id, [FromBody]AssociateAdministrativeContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.AssociateAdministrativeContactAsync(id, model);
            return Ok();
        }
    }
}
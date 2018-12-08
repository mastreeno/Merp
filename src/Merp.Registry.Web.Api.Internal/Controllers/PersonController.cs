using System;
using System.Threading.Tasks;
using Acl.RegistryResolutionServices;
using Merp.Registry.Web.Api.Internal.Models.Person;
using Merp.Registry.Web.Api.Internal.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Registry.Web.Api.Internal.Controllers
{
    [Authorize]
    public class PersonController : ControllerBase
    {
        public PersonControllerWorkerServices WorkerServices { get; private set; }

        public Resolver ResolverServiceProxy { get; private set; }

        public PersonController(PersonControllerWorkerServices workerServices, Resolver resolverServiceProxy)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
            ResolverServiceProxy = resolverServiceProxy ?? throw new ArgumentNullException(nameof(resolverServiceProxy));
        }

        [HttpGet]
        public IActionResult SearchByPattern(string query)
        {
            var viewModel = WorkerServices.SearchPeopleByPattern(query);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await WorkerServices.RegisterPersonAsync(model);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> LookupByVatNumber(string vatNumber, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(vatNumber) || string.IsNullOrWhiteSpace(countryCode))
            {
                return BadRequest();
            }

            try
            {
                var personInformation = await ResolverServiceProxy.LookupPersonInfoByVatNumberAsync(countryCode, vatNumber);
                if (personInformation == null)
                {
                    return NotFound();
                }

                return Ok(personInformation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
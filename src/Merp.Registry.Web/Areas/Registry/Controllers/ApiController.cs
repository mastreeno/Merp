using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Merp.Web.Site.Areas.Registry.WorkerServices;
using Merp.Web.Site.Areas.Registry.Models;
using Microsoft.AspNetCore.Authorization;
using Acl.RegistryResolutionServices;

namespace Merp.Web.Site.Areas.Registry.Controllers
{
    [Area("Registry")]
    [Authorize]
    public class ApiController : Controller
    {
        public ApiControllerWorkerServices WorkerServices { get; private set; }
        public Resolver ViesServiceProxy { get; private set; }

        public ApiController(ApiControllerWorkerServices workerServices, Resolver viesServiceProxy)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
            ViesServiceProxy = viesServiceProxy ?? throw new ArgumentNullException(nameof(viesServiceProxy));
        }

        [HttpGet]
        public IEnumerable<object> GetPartyInfoByPattern(string text)
        {
            var model = WorkerServices.GetPartyNamesByPattern(text);
            return model;
        }

        [HttpGet]
        public PartyInfo GetPartyInfoById(int id)
        {
            var model = WorkerServices.GetPartyInfoById(id);
            return model;
        }

        [HttpGet]
        public PartyInfo GetPartyInfoByUid(Guid id)
        {
            var model = WorkerServices.GetPartyInfoById(id);
            return model;
        }

        [HttpGet]
        public IEnumerable<object> GetPersonInfoByPattern(string text)
        {
            var model = WorkerServices.GetPersonNamesByPattern(text);
            return model;
        }

        [HttpGet]
        public PartyInfo GetPersonInfoById(int id)
        {
            var model = WorkerServices.GetPersonInfoByPattern(id);
            return model;
        }

        [HttpGet]
        public async Task<IActionResult> LookupCompanyInfoByViesService(string vatNumber, string countryCode)
        {            
            if (string.IsNullOrWhiteSpace(vatNumber) || string.IsNullOrWhiteSpace(countryCode))
            {
                return BadRequest();
            }

            try
            {
                var companyInformation = await ViesServiceProxy.LookupCompanyInfoByVatNumberAsync(countryCode, vatNumber);
                if (companyInformation == null)
                {
                    return NotFound();
                }

                return Ok(companyInformation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> LookupPersonInfoByViesService(string vatNumber, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(vatNumber) || string.IsNullOrWhiteSpace(countryCode))
            {
                return BadRequest();
            }

            try
            {
                var personInformation = await ViesServiceProxy.LookupPersonInfoByVatNumberAsync(countryCode, vatNumber);
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
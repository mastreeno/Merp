using Merp.Registry.Web.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Controllers
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
        public IActionResult Search(string query, string partyType, string city, string postalCode, string orderBy, string orderDirection, int page = 1, int size = 20)
        {
            var viewModel = WorkerServices.SearchParties(query, partyType, city, postalCode, orderBy, orderDirection, page, size);
            return Ok(viewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Unlist(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Party cannot be empty");
            }

            await WorkerServices.UnlistParty(id);
            return Ok();
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
    }
}

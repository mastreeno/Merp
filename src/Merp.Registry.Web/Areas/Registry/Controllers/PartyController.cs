using Merp.Web.Site.Areas.Registry.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.Registry.WorkerServices;
using System.Collections.Generic;
using Merp.Web.Site.Areas.Registry.Models.Party;

namespace Merp.Web.Site.Areas.Registry.Controllers
{
    [Area("Registry")]
    [Authorize(Roles = "Registry")]
    public class PartyController : Controller
    {
        public PartyControllerWorkerServices WorkerServices { get; private set; }

        public PartyController(PartyControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public ActionResult Detail(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();
            switch (WorkerServices.GetDetailViewModel(id.Value))
            {
                case "Company":
                    return Redirect(UrlBuilder.Registry.CompanyInfo(id.Value));
                case "Person":
                    return Redirect(UrlBuilder.Registry.PersonInfo(id.Value));
                default:
                    return RedirectToAction("Search");
            }
        }
        
        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<GetPartiesViewModel.PartyDescriptor> GetParties(string query, string partyType, string city, string postalCode, string orderBy, string orderDirection, int page = 1, int size = 20)
        {
            var model = WorkerServices.GetParties(query, partyType, city, postalCode, orderBy, orderDirection, page, size);
            Response.Headers.Add("size", size.ToString());
            Response.Headers.Add("total", model.TotalNumberOfParties.ToString());
            return model.Parties;
        }

        [HttpPost]
        public IActionResult Unlist(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("The party is required");
            }

            WorkerServices.UnlistParty(id);
            return Ok();
        }
    }
}
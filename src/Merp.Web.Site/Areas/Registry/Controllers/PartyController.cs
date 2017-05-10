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
            if(workerServices==null)
                throw new ArgumentNullException(nameof(workerServices));

            WorkerServices = workerServices;
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

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<GetPartiesViewModel> GetParties(string query, string partyType, string city, string orderBy, string orderDirection)
        {
            var model = WorkerServices.GetParties(query, partyType, city, orderBy, orderDirection);
            return model;
        }


    }
}
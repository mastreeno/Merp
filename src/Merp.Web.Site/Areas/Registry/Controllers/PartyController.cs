using Merp.Web.Site.Areas.Registry.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.Registry.WorkerServices;
using Merp.Web.Site.Support;

namespace Merp.Web.Site.Areas.Registry.Controllers
{
    [Area("Registry")]
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
        public ActionResult GetParties(string query)
        {
            var model = WorkerServices.GetParties(query);
            return Json(model);
        }

        [HttpGet]
        public ActionResult GetPartyInfoByPattern(string text)
        {
            var model = WorkerServices.GetPartyNamesByPattern(text);
            return Json(model);
        }

        [HttpGet]
        public ActionResult GetPartyInfoById(int id)
        {
            var model = WorkerServices.GetPartyInfoByPattern(id);
            return Json(model);
        }

        [HttpGet]
        public ActionResult GetPersonInfoByPattern(string text)
        {
            var model = WorkerServices.GetPersonNamesByPattern(text);
            return Json(model);
        }

        [HttpGet]
        public ActionResult GetPersonInfoById(int id)
        {
            var model = WorkerServices.GetPersonInfoByPattern(id);
            return Json(model);
        }
    }
}
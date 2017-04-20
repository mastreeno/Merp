using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Merp.Web.Site.Areas.Registry.WorkerServices;
using Merp.Web.Site.Areas.Registry.Models;
using Microsoft.AspNetCore.Authorization;

namespace Merp.Web.Site.Areas.Registry.Controllers
{
    [Area("Registry")]
    [Authorize]
    public class ApiController : Controller
    {
        public ApiControllerWorkerServices WorkerServices { get; private set; }

        public ApiController(ApiControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
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
            var model = WorkerServices.GetPartyInfoByPattern(id);
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
    }
}
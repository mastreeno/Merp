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

        [HttpGet]
        public object CheckVat(string vatNumber, string countryCode = "IT")
        {
            System.Threading.Thread.Sleep(2000);
            if(vatNumber == "error")
            {
                throw new Exception("remote server error");
            }
            return new {
                countryCode = "IT",
                vatNumber = "12363410155",
                requestDate = DateTime.Now,
                valid = vatNumber != "fail",
                name = "COCA-COLA HBC ITALIA SRL",
                address = "PIAZZA INDRO MONTANELLI N 30 20099 SESTO SAN GIOVANNI MI"
            };
        }
    }
}
using System;
using System.Globalization;
using System.Linq;
using Merp.Registry.Web.Api.Internal.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Registry.Web.Api.Internal.Controllers
{
    [Authorize]
    public class CountriesController : ControllerBase
    {
        public CountriesControllerWorkerServices WorkerServices { get; private set; }

        public CountriesController(CountriesControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            var language = Request.GetTypedHeaders().AcceptLanguage.FirstOrDefault()?.Value.Value;
            var currentCulture = CultureInfo.CurrentCulture;
            if (!string.IsNullOrWhiteSpace(language))
            {
                currentCulture = new CultureInfo(language);
            }

            var countries = WorkerServices.GetCountries(currentCulture);
            return Ok(countries);
        }
    }
}
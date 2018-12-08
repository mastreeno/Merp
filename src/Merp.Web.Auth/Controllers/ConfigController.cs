using System.Globalization;
using System.Linq;
using Merp.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merp.Web.Auth.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ConfigController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLocalization(string controllerResourceName, string actionResourceName)
        {
            var language = Request.GetTypedHeaders().AcceptLanguage.FirstOrDefault()?.Value.Value;
            var currentCulture = CultureInfo.CurrentCulture;
            if (!string.IsNullOrWhiteSpace(language))
            {
                currentCulture = new CultureInfo(language);
            }

            return new LocalizationJsonResult(controllerResourceName, actionResourceName, currentCulture);
        }
    }
}
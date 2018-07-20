using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Sales.Web.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Authorize(Roles = "Sales")]
    public class ProjectController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
    }
}

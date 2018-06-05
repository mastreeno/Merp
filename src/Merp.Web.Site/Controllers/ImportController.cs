//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Merp.Web.Site.WorkerServices;
//using Microsoft.AspNetCore.Authorization;

//namespace Merp.Web.Site.Controllers
//{
//    [Authorize]
//    public class ImportController : Controller
//    {
//        public ImportControllerWorkerServices WorkerServices { get; set; }

//        public ImportController(ImportControllerWorkerServices workerServices)
//        {
//            if (workerServices == null)
//                throw new ArgumentNullException(nameof(workerServices));

//            WorkerServices = workerServices;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [Authorize]
//        [HttpGet]
//        public IActionResult Start()
//        {
//            Task.Run(() => WorkerServices.Import());
//            return Redirect("/");
//        }
//    }
//}
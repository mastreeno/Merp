using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnTime.TaskManagement.CommandStack.Commands;
using OnTime.Web.Site.Areas.App.Model.Task;

using OnTime.Web.Site.Models;
using Rebus.Bus;
using OnTime.Web.Site.Areas.App.WorkerServices;

namespace OnTime.Web.Site.Areas.App.Controllers
{
    [Area("App")]
    [Authorize]
    public class TaskController : Controller
    {
        public TaskControllerWorkerServices WorkerServices { get; private set; }

        public TaskController(TaskControllerWorkerServices workerServices)
        {
            if (workerServices == null)
                throw new ArgumentNullException(nameof(workerServices));

            WorkerServices = workerServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<IncompleteViewModel> Backlog()
        {
            return WorkerServices.GetBacklogModel();
        }

        [HttpGet]
        public IEnumerable<IncompleteViewModel> Today()
        {
            return WorkerServices.GetTodayModel();
        }

        [HttpGet]
        public IActionResult Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest();

            try
            {
                WorkerServices.Create(name);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult MarkAsComplete(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();

            try
            {
                WorkerServices.MarkAsComplete(id.Value);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult MarkAsCancelled(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();

            try
            {
                WorkerServices.MarkAsCancelled(id.Value);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
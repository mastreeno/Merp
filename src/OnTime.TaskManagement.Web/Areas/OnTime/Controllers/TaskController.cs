using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.Web.Site.Areas.OnTime.WorkerServices;
using Merp.Web.Site.Areas.OnTime.Model.Task;
using OnTime.TaskManagement.Web.Areas.OnTime.Model.Task;

namespace Merp.Web.Site.Areas.OnTime.Controllers
{
    [Area("OnTime")]
    [Authorize(Roles ="TaskManagement")]
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
        public IEnumerable<CurrentTaskModel> Backlog()
        {
            return WorkerServices.GetBacklogModel();
        }

        [HttpGet]
        public IEnumerable<CurrentTaskModel> NextSevenDays()
        {
            return WorkerServices.GetNextSevenDaysModel();
        }

        [HttpGet]
        public IEnumerable<CurrentTaskModel> Today()
        {
            return WorkerServices.GetTodayModel();
        }

        [HttpPost]
        public IActionResult Add([FromForm] AddModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            try
            {
                var taskId = WorkerServices.Add(model.Name);
                return Ok(taskId);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Update(Guid id, UpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                WorkerServices.Update(id, model.Name, model.Priority, model.JobOrderId);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Cancel(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();

            try
            {
                WorkerServices.Cancel(id.Value);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
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


    }
}
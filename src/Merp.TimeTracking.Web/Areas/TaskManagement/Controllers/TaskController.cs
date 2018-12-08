using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Merp.TimeTracking.Web.Areas.TaskManagement.WorkerServices;
using Merp.TimeTracking.Web.Areas.TaskManagement.Models.Task;
using Microsoft.AspNetCore.Cors;

namespace Merp.TimeTracking.Web.Areas.TaskManagement.Controllers
{
    [Area("TaskManagement")]
    [Authorize()]
    public class TaskController : Controller
    {
        public TaskControllerWorkerServices WorkerServices { get; private set; }

        public TaskController(TaskControllerWorkerServices workerServices)
        {
            WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
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

        [HttpGet]
        public object PriorityOptions()
        {
            return Enum.GetValues(typeof(global::Merp.TimeTracking.TaskManagement.QueryStack.Model.TaskPriority));
        }

        [HttpGet]
        public IEnumerable<Guid> JobOrders()
        {
            return WorkerServices.GetJobOrders();
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

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
        public IActionResult Update(Guid id, [FromBody] UpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                WorkerServices.Update(id, model);
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
        public IActionResult MarkAsComplete(Guid id)
        {
            try
            {
                WorkerServices.MarkAsComplete(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
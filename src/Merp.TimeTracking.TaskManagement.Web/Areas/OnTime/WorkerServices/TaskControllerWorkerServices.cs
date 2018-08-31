using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Rebus.Bus;
using Merp.TimeTracking.TaskManagement.QueryStack;
using Merp.TimeTracking.TaskManagement.QueryStack.Model.Extensions;
using Merp.TimeTracking.TaskManagement.CommandStack.Commands;
using Merp.Web.Site.Models;
using Merp.Web.Site.Areas.OnTime.Model.Task;
using Merp.TimeTracking.TaskManagement.CommandStack.Model;
using Merp.TimeTracking.TaskManagement.Web.Areas.OnTime.Model.Task;

namespace Merp.Web.Site.Areas.OnTime.WorkerServices
{
    public class TaskControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; private set; }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public IHttpContextAccessor ContextAccessor { get; private set; }

        public TaskControllerWorkerServices(IBus bus, IDatabase database, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
        {
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            Database = database ?? throw new ArgumentNullException(nameof(database));
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public IEnumerable<CurrentTaskModel> GetBacklogModel()
        {
            var currentUserId = GetCurrentUserId();
            var model = (from t in Database.Tasks.Backlog(currentUserId)
                         select new CurrentTaskModel
                         {
                             TaskId = t.Id,
                             Name = t.Name,
                             Done = false,
                             Priority = t.Priority,
                             JobOrderId = t.JobOrderId
                         }).ToArray();
            return model;
        }

        public IEnumerable<CurrentTaskModel> GetTodayModel()
        {
            var currentUserId = GetCurrentUserId();
            var model = (from t in Database.Tasks
                                            .Today(currentUserId)
                         select new CurrentTaskModel
                         {
                             TaskId = t.Id,
                             Name = t.Name,
                             Done = false,
                             Priority = t.Priority,
                             JobOrderId = t.JobOrderId
                         }).ToArray();
            return model;
        }

        public IEnumerable<CurrentTaskModel> GetNextSevenDaysModel()
        {
            var threashold = DateTime.Now.AddDays(8);
            var currentUserId = GetCurrentUserId();
            var model = (from t in Database.Tasks
                                            .Backlog(currentUserId)
                         select new CurrentTaskModel
                         {
                             TaskId = t.Id,
                             Name = t.Name,
                             Done = false,
                             Priority = t.Priority,
                             JobOrderId = t.JobOrderId
                         }).ToArray();
            return model;
        }

        public Guid Add(string taskName)
        {
            var taskId = Guid.NewGuid();
            var cmd = new AddTaskCommand()
            {
                TaskId = taskId,
                UserId = GetCurrentUserId(),
                Name = taskName
            };
            Bus.Send(cmd);

            return taskId;
        }

        public void Cancel(Guid taskId)
        {
            var cmd = new CancelTaskCommand()
            {
                TaskId = taskId,
                UserId = GetCurrentUserId()
            };
            Bus.Send(cmd);
        }

        public void Update(Guid taskId, string taskName, global::Merp.TimeTracking.TaskManagement.QueryStack.Model.TaskPriority priority, Guid? jobOrderId)
        {
            var cmd = new UpdateTaskCommand(
                taskId: taskId,
                userId: GetCurrentUserId(),
                name: taskName,
                priority: priority.Convert(),
                jobOrderId: jobOrderId
            );
            Bus.Send(cmd);
        }

        public void MarkAsComplete(Guid taskId)
        {
            if (taskId == Guid.Empty)
                throw new ArgumentException("A valid taskId must be specified", nameof(taskId));

            var currentUserId = GetCurrentUserId();
            var taskBelongsToRequestingUser = Database.Tasks.Backlog(currentUserId).Any(t => t.Id == taskId);
            if (!taskBelongsToRequestingUser)
                throw new InvalidOperationException("The specified task does not belong to current user.");

            var cmd = new MarkTaskAsCompletedCommand()
            {
                TaskId = taskId,
                UserId = GetCurrentUserId()
            };
            Bus.Send(cmd);
        }

        private Guid GetCurrentUserId()
        {
            var user = ContextAccessor.HttpContext.User;
            var currentUser = UserManager.GetUserAsync(user).Result;
            var userId = currentUser.Id;
            return Guid.Parse(userId);
        }
    }
}

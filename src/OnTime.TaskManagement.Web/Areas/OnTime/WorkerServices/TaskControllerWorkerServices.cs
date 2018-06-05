using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Rebus.Bus;
using OnTime.TaskManagement.QueryStack;
using OnTime.TaskManagement.QueryStack.Model.Extensions;
using OnTime.TaskManagement.CommandStack.Commands;
using Merp.Web.Site.Models;
using Merp.Web.Site.Areas.OnTime.Model.Task;

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

        public IEnumerable<IncompleteViewModel> GetBacklogModel()
        {
            var currentUserId = GetCurrentUserId();
            var model = (from t in Database.Tasks.Backlog(currentUserId)
                         select new IncompleteViewModel
                         {
                             TaskId = t.Id,
                             Name = t.Name,
                             Done = false
                         }).ToArray();
            return model;
        }

        public IEnumerable<IncompleteViewModel> GetTodayModel()
        {
            var currentUserId = GetCurrentUserId();
            var model = (from t in Database.Tasks
                                            .Backlog(currentUserId)
                         select new IncompleteViewModel
                         {
                             TaskId = t.Id,
                             Name = t.Name,
                             Done = false
                         }).ToArray();
            return model;
        }

        public void MarkAsCancelled(Guid taskId)
        {
            var cmd = new CancelTaskCommand()
            {
                TaskId = taskId,
                UserId = GetCurrentUserId()
            };
            Bus.Send(cmd);
        }

        public void Create(string taskName)
        {
            var cmd = new CreateTaskCommand()
            {
                Name = taskName,
                UserId = GetCurrentUserId()
            };
            Bus.Send(cmd);
        }

        public void MarkAsComplete(Guid taskId)
        {
            var currentUserId = GetCurrentUserId();
            var taskBelongsToRequestingUser = Database.Tasks.OfUser(currentUserId).Current().Any(t => t.Id == taskId);
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

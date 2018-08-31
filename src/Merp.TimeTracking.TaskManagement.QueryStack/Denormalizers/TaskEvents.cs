using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merp.TimeTracking.TaskManagement.CommandStack.Events;
using Merp.TimeTracking.TaskManagement.QueryStack.Model;
using OTask = Merp.TimeTracking.TaskManagement.QueryStack.Model.Task;
using Merp.TimeTracking.TaskManagement.QueryStack.Model.Extensions;

namespace Merp.TimeTracking.TaskManagement.QueryStack.Denormalizers
{
    public class TaskEvents : 
        IHandleMessages<TaskCancelledEvent>,
        IHandleMessages<TaskCompletedEvent>,
        IHandleMessages<TaskAddedEvent>,
        IHandleMessages<TaskUpdatedEvent>
    {
        public TaskManagementDbContext ActiveDbContext { get; private set; }

        public TaskEvents(TaskManagementDbContext activeDbContext)
        {
            if (activeDbContext == null)
                throw new ArgumentNullException(nameof(activeDbContext));

            ActiveDbContext = activeDbContext;
        }

        public async System.Threading.Tasks.Task Handle(TaskAddedEvent message)
        {
            var t = new OTask();
            t.Id = message.TaskId;
            t.DateOfCreation = message.DateOfCreation;
            t.IsCompleted = false;
            t.Name = message.TaskName;
            t.UserId = message.UserId;

            ActiveDbContext.Tasks.Add(t);
            await ActiveDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Handle(TaskCompletedEvent message)
        {
            var t = ActiveDbContext.Tasks.Find(message.TaskId);
            t.IsCompleted = true;
            t.DateOfCompletion = message.DateOfCompletion;
            await ActiveDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Handle(TaskCancelledEvent message)
        {
            var t = ActiveDbContext.Tasks.Find(message.TaskId);
            t.DateOfCancellation = message.DateOfCancellation;
            await ActiveDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Handle(TaskUpdatedEvent message)
        {
            var t = ActiveDbContext.Tasks.Find(message.TaskId);
            t.Name = message.Name;
            t.Priority = message.Priority.Convert();
            t.JobOrderId = message.JobOrderId;
            await ActiveDbContext.SaveChangesAsync();
        }
    }
}

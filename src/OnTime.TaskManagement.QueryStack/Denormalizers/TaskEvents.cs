using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnTime.TaskManagement.CommandStack.Events;
using OnTime.TaskManagement.QueryStack.Model;
using OTask = OnTime.TaskManagement.QueryStack.Model.Task;

namespace OnTime.TaskManagement.QueryStack.Denormalizers
{
    public class TaskEvents : 
        IHandleMessages<TaskDeletedEvent>,
        IHandleMessages<TaskCompletedEvent>,
        IHandleMessages<TaskAddedEvent>,
        IHandleMessages<TaskRenamedEvent>,
        IHandleMessages<DueDateSetForTaskEvent>,
        IHandleMessages<DueDateRemovedFromTaskEvent>
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
            t.Name = message.TaskText;
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

        public async System.Threading.Tasks.Task Handle(TaskDeletedEvent message)
        {
            var t = ActiveDbContext.Tasks.Find(message.TaskId);
            ActiveDbContext.Tasks.Remove(t);
            await ActiveDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Handle(TaskRenamedEvent message)
        {
            var t = ActiveDbContext.Tasks.Find(message.TaskId);
            t.Name = message.TaskName;
            await ActiveDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Handle(DueDateSetForTaskEvent message)
        {
            var t = ActiveDbContext.Tasks.Find(message.TaskId);
            t.DueDate = message.DueDate;
            await ActiveDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Handle(DueDateRemovedFromTaskEvent message)
        {
            var t = ActiveDbContext.Tasks.Find(message.TaskId);
            t.DueDate = null;
            await ActiveDbContext.SaveChangesAsync();
        }
    }
}

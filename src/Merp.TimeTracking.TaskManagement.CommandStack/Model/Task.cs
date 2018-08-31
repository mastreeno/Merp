using MementoFX.Domain;
using Merp.TimeTracking.TaskManagement.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.TimeTracking.TaskManagement.CommandStack.Model
{
    public class Task : Aggregate,
                        IApplyEvent<TaskAddedEvent>,
                        IApplyEvent<TaskCompletedEvent>,
                        IApplyEvent<TaskCancelledEvent>,
                        IApplyEvent<TaskUpdatedEvent>
    {
        protected Task()
        {

        }
        public Guid CreatorId { get; private set; }

        public DateTime DateOfCreation { get; private set; }

        public DateTime? DateOfCompletion { get; private set; }

        public DateTime? DateOfCancellation { get; private set; }

        public DateTime? DueDate { get; private set; }

        public string Name { get; private set; }

        public Guid? JobOrderId { get; private set; }

        public TaskPriority Priority { get; private set; }

        public void ApplyEvent(TaskAddedEvent @event)
        {
            this.Id = @event.TaskId;
            this.DateOfCreation = @event.DateOfCreation;
            this.Name = @event.TaskName;
            this.CreatorId = @event.UserId;
            this.Priority = @event.Priority;
        }

        public void ApplyEvent(TaskCompletedEvent @event)
        {
            this.DateOfCompletion = @event.DateOfCompletion;
        }

        public void ApplyEvent(TaskCancelledEvent @event)
        {
            this.DateOfCancellation = @event.DateOfCancellation;
        }

        public void ApplyEvent(TaskUpdatedEvent @event)
        {
            this.Name = @event.Name;
            this.Priority = @event.Priority;
            this.JobOrderId = @event.JobOrderId;
        }

        public void Cancel(Guid userId)
        {
            if (userId != this.CreatorId)
                throw new ArgumentException("A task can be cancelled by its creator only", nameof(userId));
            if (this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't cancel a closed task.");
            if (this.DateOfCancellation.HasValue)
                throw new InvalidOperationException("Can't cancel a task twice.");

            var e = new TaskCancelledEvent()
            {
                TaskId = this.Id,
                DateOfCancellation = DateTime.Now,
                UserId = userId
            };
            RaiseEvent(e);
        }

        public void MarkAsCompleted(Guid userId)
        {
            if (userId != this.CreatorId)
                throw new ArgumentException("A task can only be marked as completed by its creator", nameof(userId));
            if (this.DateOfCancellation.HasValue)
                throw new InvalidOperationException("Can't mark a cancelled task as completed.");
            if (this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't mark a closed task as completed.");

            var e = new TaskCompletedEvent()
            {
                DateOfCompletion = DateTime.Now,
                TaskId = this.Id,
                UserId = userId
            };
            RaiseEvent(e);
        }

        public void Update(string text, TaskPriority priority, Guid? jobOrderId)
        {
            if (this.DateOfCancellation.HasValue)
                throw new InvalidOperationException("Can't update a cancelled task.");
            if (this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't update a completed task.");
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("A task must have a non-null name.", nameof(text));

            var e = new TaskUpdatedEvent()
            {
                TaskId = this.Id,
                Name = text,
                Priority = priority,
                JobOrderId = jobOrderId
            };
            RaiseEvent(e);
        }

        public static class Factory
        {
            public static Task Create(Guid taskId, Guid userId, string name)
            {
                if (userId == Guid.Empty)
                    throw new ArgumentException("Invalid user Id", nameof(userId));
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("A task must have a name.", nameof(name));

                var e = new TaskAddedEvent()
                {
                    TaskId = taskId,
                    DateOfCreation = DateTime.Now,
                    TaskName = name,
                    UserId = userId,
                    Priority = TaskPriority.Standard
                };
                var task = new Task();
                task.RaiseEvent(e);
                return task;
            }
        }
    }
}

using MementoFX.Domain;
using OnTime.TaskManagement.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTime.TaskManagement.CommandStack.Model
{
    public class Task : Aggregate,
                        IApplyEvent<TaskCreatedEvent>,
                        IApplyEvent<TaskCompletedEvent>,
                        IApplyEvent<TaskDeletedEvent>,
                        IApplyEvent<TaskRenamedEvent>,
                        IApplyEvent<TaskReactivatedEvent>,
                        IApplyEvent<DueDateSetForTaskEvent>,
                        IApplyEvent<DueDateRemovedFromTaskEvent>
    {
        protected Task()
        {

        }

        public DateTime DateOfCreation { get; private set; }

        public DateTime? DateOfCompletion { get; private set; }

        public DateTime? DateOfCancellation { get; private set; }

        public DateTime? DueDate { get; set; }

        public string Name { get; private set; }

        public Guid CreatorId { get; private set; }

        public void ApplyEvent(TaskCreatedEvent @event)
        {
            this.Id = @event.TaskId;
            this.DateOfCreation = @event.DateOfCreation;
            this.Name = @event.TaskName;
            this.CreatorId = @event.UserId;
        }

        public void ApplyEvent(TaskCompletedEvent @event)
        {
            this.DateOfCompletion = @event.DateOfCompletion;
        }

        public void ApplyEvent(TaskDeletedEvent @event)
        {
            this.DateOfCancellation = @event.DateOfCancellation;
        }

        public void ApplyEvent(TaskRenamedEvent @event)
        {
            this.Name = @event.TaskName;
        }

        public void ApplyEvent(TaskReactivatedEvent @event)
        {
            this.DateOfCancellation = null;
            this.DateOfCompletion = null;
        }

        public void ApplyEvent(DueDateSetForTaskEvent @event)
        {
            this.DueDate = @event.DueDate;
        }

        public void ApplyEvent(DueDateRemovedFromTaskEvent @event)
        {
            this.DueDate = null;
        }

        public void Cancel(Guid userId)
        {
            if (userId != this.CreatorId)
                throw new ArgumentException("A task can be cancelled by its creator only", nameof(userId));
            if (this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't cancel a closed task.");
            if (this.DateOfCancellation.HasValue)
                throw new InvalidOperationException("Can't cancel a task twice.");

            var e = new TaskDeletedEvent()
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
                throw new ArgumentException("A task can be marked as completed by its creator only", nameof(userId));
            if (this.DateOfCancellation.HasValue)
                throw new InvalidOperationException("Can't mark as completed a cancelled task.");
            if (this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't mark as completed a closed task.");

            var e = new TaskCompletedEvent()
            {
                DateOfCompletion = DateTime.Now,
                TaskId = this.Id,
                UserId = userId
            };
            RaiseEvent(e);
        }

        public void Rename(string proposedName)
        {
            if (this.DateOfCancellation.HasValue)
                throw new InvalidOperationException("Can't mark as completed a cancelled task.");
            if (this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't mark as completed a closed task.");
            if (string.IsNullOrWhiteSpace(proposedName))
                throw new ArgumentException("A task must have a non-null name.", nameof(proposedName));

            var e = new TaskRenamedEvent()
            {
                TaskId = this.Id,
                TaskName = proposedName
            };
            RaiseEvent(e);
        }

        public void Reactivate()
        {
            if (!this.DateOfCancellation.HasValue && !this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't reactivate an already active task.");
            var e = new TaskReactivatedEvent()
            {
                TaskId = this.Id
            };
            RaiseEvent(e);
        }

        public void SetDueDate(DateTime dueDate)
        {
            if (this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't set a due date for a completed task");
            if (this.DateOfCancellation.HasValue)
                throw new InvalidOperationException("Can't set a due date for a cancelled task");
            if (dueDate < this.DateOfCreation)
                throw new ArgumentException("The due date should be set later than the creation date.", nameof(dueDate));
            var e = new DueDateSetForTaskEvent()
            {
                DueDate = dueDate,
                TaskId = this.Id
            };
            RaiseEvent(e);
        }

        public void RemoveDueDate()
        {
            if (this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't remove due date for a completed task");
            if (this.DateOfCancellation.HasValue)
                throw new InvalidOperationException("Can't remove due date for a cancelled task");
            if (!this.DueDate.HasValue)
                throw new InvalidOperationException("Can't remove due date for a task that already doesn't have any due date");
            var e = new DueDateRemovedFromTaskEvent
            {
                TaskId = this.Id
            };
            RaiseEvent(e);
        }

        public static class Factory
        {
            public static Task Create(Guid userId, string name)
            {
                if (userId == Guid.Empty)
                    throw new ArgumentException("Invalid user Id", nameof(userId));
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("A task must have a name.", nameof(name));

                var e = new TaskCreatedEvent()
                {
                    TaskId = Guid.NewGuid(),
                    DateOfCreation = DateTime.Now,
                    TaskName = name,
                    UserId = userId
                };
                var task = new Task();
                task.RaiseEvent(e);
                return task;
            }
        }
    }
}

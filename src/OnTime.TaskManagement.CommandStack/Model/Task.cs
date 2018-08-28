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
                        IApplyEvent<TaskAddedEvent>,
                        IApplyEvent<TaskCompletedEvent>,
                        IApplyEvent<TaskDeletedEvent>,
                        IApplyEvent<TaskUpdatedEvent>
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

        public void ApplyEvent(TaskAddedEvent @event)
        {
            this.Id = @event.TaskId;
            this.DateOfCreation = @event.DateOfCreation;
            this.Name = @event.TaskText;
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

        public void ApplyEvent(TaskUpdatedEvent @event)
        {
            this.Name = @event.Text;
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

        public void Update(string proposedName)
        {
            if (this.DateOfCancellation.HasValue)
                throw new InvalidOperationException("Can't mark as completed a cancelled task.");
            if (this.DateOfCompletion.HasValue)
                throw new InvalidOperationException("Can't mark as completed a closed task.");
            if (string.IsNullOrWhiteSpace(proposedName))
                throw new ArgumentException("A task must have a non-null name.", nameof(proposedName));

            var e = new TaskUpdatedEvent()
            {
                TaskId = this.Id,
                Text = proposedName
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

                var e = new TaskAddedEvent()
                {
                    TaskId = Guid.NewGuid(),
                    DateOfCreation = DateTime.Now,
                    TaskText = name,
                    UserId = userId
                };
                var task = new Task();
                task.RaiseEvent(e);
                return task;
            }
        }
    }
}

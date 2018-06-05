//using MementoFX.Messaging.Postie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using MementoFX.Persistence;
using OnTime.TaskManagement.CommandStack.Commands;
using OnTime.TaskManagement.CommandStack.Events;
using OnTime.TaskManagement.CommandStack.Model;
using OTask = OnTime.TaskManagement.CommandStack.Model.Task;

namespace OnTime.TaskManagement.CommandStack.Sagas
{
    public class TaskLifecycleSaga : Saga<TaskLifecycleSaga.TaskLifecycleSagaData>,
        IAmInitiatedBy<CreateTaskCommand>,
        IHandleMessages<MarkTaskAsCompletedCommand>,
        IHandleMessages<CancelTaskCommand>,
        IHandleMessages<RenameTaskCommand>,
        IHandleMessages<ReactivateTaskCommand>,
        IHandleMessages<SetTaskDueDateCommand>,
        IHandleMessages<RemoveTaskDueDateCommand>,
        IHandleMessages<TaskCompletedEvent>,
        IHandleMessages<TaskLifecycleSaga.TaskCreatedTimeout>
    {
        private readonly IRepository _repository;
        private readonly IBus _bus;

        public TaskLifecycleSaga(IRepository repository, IBus bus)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        protected override void CorrelateMessages(ICorrelationConfig<TaskLifecycleSagaData> config)
        {
            config.Correlate<CreateTaskCommand>(
                message => message.TaskId,
                sagaData => sagaData.TaskId);

            config.Correlate<MarkTaskAsCompletedCommand>(
                message => message.TaskId,
                sagaData => sagaData.TaskId);

            config.Correlate<CancelTaskCommand>(
                message => message.TaskId,
                sagaData => sagaData.TaskId);

            config.Correlate<RenameTaskCommand>(
                message => message.TaskId,
                sagaData => sagaData.TaskId);

            config.Correlate<ReactivateTaskCommand>(
                message => message.TaskId,
                sagaData => sagaData.TaskId);

            config.Correlate<SetTaskDueDateCommand>(
                message => message.TaskId,
                sagaData => sagaData.TaskId);

            config.Correlate<RemoveTaskDueDateCommand>(
                message => message.TaskId,
                sagaData => sagaData.TaskId);

            config.Correlate<TaskCompletedEvent>(
                message => message.TaskId,
                sagaData => sagaData.TaskId);

            config.Correlate<TaskCreatedTimeout>(
                message => message.TaskId,
                sagaData => sagaData.TaskId);
        }

        public async System.Threading.Tasks.Task Handle(CreateTaskCommand message)
        {
            var task = OTask.Factory.Create(message.UserId, message.Name);
            await _repository.SaveAsync(task);
            this.Data.TaskId = task.Id;
        }

        public async System.Threading.Tasks.Task Handle(RenameTaskCommand message)
        {
            var task = _repository.GetById<OTask>(message.TaskId);
            task.Rename(message.ProposedName);
            await _repository.SaveAsync(task);
        }

        public async System.Threading.Tasks.Task Handle(MarkTaskAsCompletedCommand message)
        {
            var task = _repository.GetById<OTask>(message.TaskId);
            task.MarkAsCompleted(message.UserId);
            await _repository.SaveAsync(task);
        }

        public async System.Threading.Tasks.Task Handle(CancelTaskCommand message)
        {
            var task = _repository.GetById<OTask>(message.TaskId);
            task.Cancel(message.UserId);
            await _repository.SaveAsync(task);
            this.MarkAsComplete();
        }

        public async System.Threading.Tasks.Task Handle(ReactivateTaskCommand message)
        {
            var task = _repository.GetById<OTask>(message.TaskId);
            task.Reactivate();
            await _repository.SaveAsync(task);
        }

        public async System.Threading.Tasks.Task Handle(SetTaskDueDateCommand message)
        {
            var task = _repository.GetById<OTask>(message.TaskId);
            task.SetDueDate(message.DueDate);
            await _repository.SaveAsync(task);
        }

        public async System.Threading.Tasks.Task Handle(RemoveTaskDueDateCommand message)
        {
            var task = _repository.GetById<OTask>(message.TaskId);
            task.RemoveDueDate();
            await  _repository.SaveAsync(task);
        }

        public async System.Threading.Tasks.Task Handle(TaskCompletedEvent message)
        {
            var msg = new TaskCreatedTimeout()
            {
                TaskId = message.TaskId
            };
            await _bus.Defer(TimeSpan.FromDays(5), msg);
        }

        public System.Threading.Tasks.Task Handle(TaskCreatedTimeout message)
        {
            return System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                var task = _repository.GetById<OTask>(message.TaskId);
                if(task.DateOfCompletion.HasValue && DateTime.Now.Subtract(task.DateOfCompletion.Value).Days >= 5) 
                    this.MarkAsComplete();
            });
        }

        public class TaskLifecycleSagaData : SagaData
        {
            public string Name { get; set; }

            public Guid TaskId { get; set; }
        }

        public class TaskCreatedTimeout
        {
            public Guid TaskId { get; set; }
        }
    }
}

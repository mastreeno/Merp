using System;
using Merp.ProjectManagement.CommandStack.Commands;
using Merp.ProjectManagement.CommandStack.Model;
using Merp.ProjectManagement.CommandStack.Services;
using MementoFX.Persistence;
using Rebus.Handlers;
using Rebus.Sagas;
using Rebus.Bus;
using System.Threading.Tasks;
using Merp.ProjectManagement.CommandStack.Events;
using System.Linq;

namespace Merp.ProjectManagement.CommandStack.Sagas
{
    public sealed class ProjectSaga : Saga<ProjectSaga.ProjectSagaData>,
        IAmInitiatedBy<ImportProjectCommand>,
        IAmInitiatedBy<RegisterProjectCommand>,
        IHandleMessages<ExtendProjectCommand>,
        IHandleMessages<MarkProjectAsCompletedCommand>
    {
        public readonly IRepository repository;
        public readonly IEventStore eventStore;
        public readonly IBus bus;
        public readonly IProjectNumberGenerator ProjectNumberGenerator;

        public ProjectSaga(IBus bus, IEventStore eventStore, IRepository repository, IProjectNumberGenerator jobOrderNumberGenerator)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            ProjectNumberGenerator = jobOrderNumberGenerator ?? throw new ArgumentNullException(nameof(jobOrderNumberGenerator));
        }

        protected override void CorrelateMessages(ICorrelationConfig<ProjectSagaData> config)
        {
            config.Correlate<ImportProjectCommand>(
                message => message.ProjectId,
                sagaData => sagaData.ProjectId);

            config.Correlate<RegisterProjectCommand>(
                message => message.ProjectId,
                sagaData => sagaData.ProjectId);

            config.Correlate<ExtendProjectCommand>(
                message => message.ProjectId,
                sagaData => sagaData.ProjectId);

            config.Correlate<MarkProjectAsCompletedCommand>(
                message => message.ProjectId,
                sagaData => sagaData.ProjectId);
        }

        public async Task Handle(ImportProjectCommand message)
        {
            var jobOrder = Project.Factory.Import(
                message.ProjectId,
                message.ProjectNumber,
                message.CustomerId,
                message.ContactPersonId,
                message.ManagerId,
                message.Price,
                message.DateOfRegistration,
                message.DateOfStart,
                message.DueDate,
                message.IsTimeAndMaterial,
                message.PurchaseOrderNumber,
                message.ProjectName,
                message.Description
            );
            await this.repository.SaveAsync(jobOrder);
            this.Data.ProjectId = jobOrder.Id;
        }

        public async Task Handle(RegisterProjectCommand message)
        {
            var jobOrder = Project.Factory.RegisterNew(
            ProjectNumberGenerator,
            message.CustomerId,
            message.ContactPersonId,
            message.ManagerId,
            message.Price,
            message.DateOfStart,
            message.DueDate,
            message.IsTimeAndMaterial,
            message.CustomerPurchaseOrderNumber,
            message.ProjectName,
            message.Description
            );
            await this.repository.SaveAsync(jobOrder);
            this.Data.ProjectId = jobOrder.Id;
        }

        public async Task Handle(ExtendProjectCommand message)
        {
            var jobOrder = repository.GetById<Project>(message.ProjectId);
            jobOrder.Extend(message.NewDueDate, message.Price);
            await repository.SaveAsync(jobOrder);
        }

        public async Task Handle(MarkProjectAsCompletedCommand message)
        {
            var jobOrder = repository.GetById<Project>(message.ProjectId);
            jobOrder.MarkAsCompleted(message.DateOfCompletion);
            await repository.SaveAsync(jobOrder);
            this.MarkAsComplete();
        }

        public class ProjectSagaData : SagaData
        {
            public Guid ProjectId { get; set; }
        }
    }
}

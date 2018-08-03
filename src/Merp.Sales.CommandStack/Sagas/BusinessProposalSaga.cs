using System;
using Merp.Sales.CommandStack.Commands;
using Merp.Sales.CommandStack.Model;
using Merp.Sales.CommandStack.Services;
using MementoFX.Persistence;
using Rebus.Handlers;
using Rebus.Sagas;
using Rebus.Bus;
using System.Threading.Tasks;
using Merp.Sales.CommandStack.Events;
using System.Linq;

namespace Merp.Sales.CommandStack.Sagas
{
    public sealed class BusinessProposalSaga : Saga<BusinessProposalSaga.BusinessProposalSagaData>,
        IAmInitiatedBy<ImportBusinessProposalCommand>,
        IAmInitiatedBy<RegisterProjectCommand>,
        IHandleMessages<MarkProjectAsCompletedCommand>
    {
        public readonly IRepository repository;
        public readonly IEventStore eventStore;
        public readonly IBus bus;
        public readonly IBusinessProposalNumberGenerator ProjectNumberGenerator;

        public BusinessProposalSaga(IBus bus, IEventStore eventStore, IRepository repository, IBusinessProposalNumberGenerator jobOrderNumberGenerator)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            ProjectNumberGenerator = jobOrderNumberGenerator ?? throw new ArgumentNullException(nameof(jobOrderNumberGenerator));
        }

        protected override void CorrelateMessages(ICorrelationConfig<BusinessProposalSagaData> config)
        {
            config.Correlate<ImportBusinessProposalCommand>(
                message => message.ProjectId,
                sagaData => sagaData.ProjectId);

            config.Correlate<RegisterProjectCommand>(
                message => message.ProjectId,
                sagaData => sagaData.ProjectId);

            config.Correlate<MarkProjectAsCompletedCommand>(
                message => message.ProjectId,
                sagaData => sagaData.ProjectId);
        }

        public async Task Handle(ImportBusinessProposalCommand message)
        {
            var jobOrder = BusinessProposal.Factory.Import(
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
            var jobOrder = BusinessProposal.Factory.RegisterNew(
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

        public async Task Handle(MarkProjectAsCompletedCommand message)
        {
            var jobOrder = repository.GetById<BusinessProposal>(message.ProjectId);
            jobOrder.MarkAsCompleted(message.DateOfCompletion);
            await repository.SaveAsync(jobOrder);
            this.MarkAsComplete();
        }

        public class BusinessProposalSagaData : SagaData
        {
            public Guid ProjectId { get; set; }
        }
    }
}

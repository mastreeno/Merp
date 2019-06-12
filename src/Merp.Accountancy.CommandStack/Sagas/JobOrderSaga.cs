using System;
using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using MementoFX.Persistence;
using Rebus.Handlers;
using Rebus.Sagas;
using Rebus.Bus;
using System.Threading.Tasks;
using Merp.Accountancy.CommandStack.Events;
using System.Linq;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public sealed class JobOrderSaga : Saga<JobOrderSaga.JobOrderSagaData>,
        IAmInitiatedBy<ImportJobOrderCommand>,
        IAmInitiatedBy<RegisterJobOrderCommand>,
        IHandleMessages<ExtendJobOrderCommand>,
        IHandleMessages<LinkIncomingCreditNoteToJobOrderCommand>,
        IHandleMessages<LinkIncomingInvoiceToJobOrderCommand>,
        IHandleMessages<LinkOutgoingCreditNoteToJobOrderCommand>,
        IHandleMessages<LinkOutgoingInvoiceToJobOrderCommand>,
        IHandleMessages<MarkJobOrderAsCompletedCommand>
    {
        public readonly IRepository repository;
        public readonly IEventStore eventStore;
        public readonly IBus bus;
        public readonly IJobOrderNumberGenerator JobOrderNumberGenerator;

        public JobOrderSaga(IBus bus, IEventStore eventStore, IRepository repository, IJobOrderNumberGenerator jobOrderNumberGenerator)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            JobOrderNumberGenerator = jobOrderNumberGenerator ?? throw new ArgumentNullException(nameof(jobOrderNumberGenerator));
        }

        protected override void CorrelateMessages(ICorrelationConfig<JobOrderSagaData> config)
        {
            config.Correlate<ImportJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<RegisterJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<ExtendJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkIncomingCreditNoteToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkIncomingInvoiceToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkOutgoingCreditNoteToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkOutgoingInvoiceToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<MarkJobOrderAsCompletedCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);
        }

        public async Task Handle(ImportJobOrderCommand message)
        {
            var jobOrder = JobOrder.Factory.Import(
                message.JobOrderId,
                message.JobOrderNumber,
                message.Customer.Id,
                message.Customer.Name,
                message.ContactPersonId,
                message.ManagerId,
                message.Price,
                message.Currency,
                message.DateOfRegistration,
                message.DateOfStart,
                message.DueDate,
                message.IsTimeAndMaterial,
                message.JobOrderName,
                message.PurchaseOrderNumber,
                message.Description,
                message.UserId
            );
            await this.repository.SaveAsync(jobOrder);
            this.Data.JobOrderId = jobOrder.Id;
        }

        public async Task Handle(RegisterJobOrderCommand message)
        {
            var jobOrder = JobOrder.Factory.RegisterNew(
            JobOrderNumberGenerator,
            message.CustomerId,
            message.CustomerName,
            message.ContactPersonId,
            message.ManagerId,
            message.Price,
            message.Currency,
            message.DateOfStart,
            message.DueDate,
            message.IsTimeAndMaterial,
            message.JobOrderName,
            message.PurchaseOrderNumber,
            message.Description,
            message.UserId
            );
            await this.repository.SaveAsync(jobOrder);
            this.Data.JobOrderId = jobOrder.Id;
        }

        public async Task Handle(ExtendJobOrderCommand message)
        {
            var jobOrder = repository.GetById<JobOrder>(message.JobOrderId);
            jobOrder.Extend(message.NewDueDate, message.Price, message.UserId);
            await repository.SaveAsync(jobOrder);
        }

        public async Task Handle(MarkJobOrderAsCompletedCommand message)
        {
            var jobOrder = repository.GetById<JobOrder>(message.JobOrderId);
            jobOrder.MarkAsCompleted(message.DateOfCompletion, message.UserId);
            await repository.SaveAsync(jobOrder);
            this.MarkAsComplete();
        }

        public async Task Handle(LinkIncomingCreditNoteToJobOrderCommand message)
        {
            var count = eventStore.Find<IncomingCreditNoteLinkedToJobOrderEvent>(e => e.CreditNoteId == message.CreditNoteId && e.JobOrderId == message.JobOrderId).Count();
            if (count > 0)
                throw new InvalidOperationException("The specified invoice is already associated to a Job Order.");
            var jobOrder = repository.GetById<JobOrder>(message.JobOrderId);
            jobOrder.LinkIncomingCreditNote(eventStore, message.CreditNoteId, message.DateOfLink, message.Amount, message.UserId);
            await repository.SaveAsync(jobOrder);
        }

        public async Task Handle(LinkIncomingInvoiceToJobOrderCommand message)
        {
            var count = eventStore.Find<IncomingInvoiceLinkedToJobOrderEvent>(e => e.InvoiceId == message.InvoiceId && e.JobOrderId == message.JobOrderId).Count();
            if (count > 0)
                throw new InvalidOperationException("The specified invoice is already associated to a Job Order.");
            var jobOrder = repository.GetById<JobOrder>(message.JobOrderId);
            jobOrder.LinkIncomingInvoice(eventStore, message.InvoiceId, message.DateOfLink, message.Amount, message.UserId);
            await repository.SaveAsync(jobOrder);
        }

        public async Task Handle(LinkOutgoingCreditNoteToJobOrderCommand message)
        {
            var count = eventStore.Find<OutgoingCreditNoteLinkedToJobOrderEvent>(e => e.CreditNoteId == message.CreditNoteId && e.JobOrderId == message.JobOrderId).Count();
            if (count > 0)
                throw new InvalidOperationException("The specified credit note is already associated to a Job Order.");
            var jobOrder = repository.GetById<JobOrder>(message.JobOrderId);
            jobOrder.LinkOutgoingCreditNote(eventStore, message.CreditNoteId, message.DateOfLink, message.Amount, message.UserId);
            await repository.SaveAsync(jobOrder);
        }

        public async Task Handle(LinkOutgoingInvoiceToJobOrderCommand message)
        {
            var count = eventStore.Find<OutgoingInvoiceLinkedToJobOrderEvent>(e => e.InvoiceId == message.InvoiceId && e.JobOrderId == message.JobOrderId).Count();
            if (count > 0)
                throw new InvalidOperationException("The specified invoice is already associated to a Job Order.");
            var jobOrder = repository.GetById<JobOrder>(message.JobOrderId);
            jobOrder.LinkOutgoingInvoice(eventStore, message.InvoiceId, message.DateOfLink, message.Amount, message.UserId);
            await repository.SaveAsync(jobOrder);
        }

        public class JobOrderSagaData : SagaData
        {
            public Guid JobOrderId { get; set; }
        }
    }
}

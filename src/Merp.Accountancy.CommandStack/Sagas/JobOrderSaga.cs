using System;
using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using Memento.Persistence;
using Rebus.Handlers;
using Rebus.Sagas;
using Rebus.Bus;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public sealed class JobOrderSaga : Saga<JobOrderSaga.FixedPriceJobOrderSagaData>,
        IAmInitiatedBy<RegisterJobOrderCommand>,
        IHandleMessages<ExtendJobOrderCommand>,
        IHandleMessages<LinkIncomingInvoiceToJobOrderCommand>,
        IHandleMessages<LinkOutgoingInvoiceToJobOrderCommand>,
        IHandleMessages<MarkJobOrderAsCompletedCommand>
    {
        private readonly IRepository _repository;
        private readonly IEventStore _eventStore;
        private readonly IBus _bus;
        public IJobOrderNumberGenerator JobOrderNumberGenerator { get; private set; }

        public JobOrderSaga(IBus bus, IEventStore eventStore, IRepository repository, IJobOrderNumberGenerator jobOrderNumberGenerator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            JobOrderNumberGenerator = jobOrderNumberGenerator ?? throw new ArgumentNullException(nameof(jobOrderNumberGenerator));
        }

        protected override void CorrelateMessages(ICorrelationConfig<FixedPriceJobOrderSagaData> config)
        {
            config.Correlate<RegisterJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<ExtendJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkIncomingInvoiceToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkOutgoingInvoiceToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<MarkJobOrderAsCompletedCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);
        }

        public Task Handle(RegisterJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = JobOrder.Factory.CreateNewInstance(
                JobOrderNumberGenerator,
                message.CustomerId,
                message.ManagerId,
                message.Price,
                message.Currency,
                message.DateOfStart,
                message.DueDate,
                message.IsTimeAndMaterial,
                message.JobOrderName,
                message.PurchaseOrderNumber,
                message.Description
                );
                this._repository.Save(jobOrder);
                this.Data.JobOrderId = jobOrder.Id;
            });
        }

        public Task Handle(ExtendJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<JobOrder>(message.JobOrderId);
                jobOrder.Extend(message.NewDueDate, message.Price);
                _repository.Save(jobOrder);
            });
        }

        public Task Handle(MarkJobOrderAsCompletedCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<JobOrder>(message.JobOrderId);
                jobOrder.MarkAsCompleted(message.DateOfCompletion);
                _repository.Save(jobOrder);
                this.MarkAsComplete();
            });
        }

        public Task Handle(LinkIncomingInvoiceToJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<JobOrder>(message.JobOrderId);
                jobOrder.LinkIncomingInvoice(_eventStore, message.InvoiceId, message.DateOfLink, message.Amount);
                _repository.Save(jobOrder);
            });
        }

        public Task Handle(LinkOutgoingInvoiceToJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<JobOrder>(message.JobOrderId);
                jobOrder.LinkOutgoingInvoice(_eventStore, message.InvoiceId, message.DateOfLink, message.Amount);
                _repository.Save(jobOrder);
            });
        }

        public class FixedPriceJobOrderSagaData : SagaData
        {
            public Guid JobOrderId { get; set; }
        }
    }
}

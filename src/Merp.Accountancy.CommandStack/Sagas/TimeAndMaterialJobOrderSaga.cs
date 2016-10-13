using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using System;
using Memento.Persistence;
using Rebus.Sagas;
using Rebus.Handlers;
using Rebus.Bus;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public class TimeAndMaterialJobOrderSaga : Saga<TimeAndMaterialJobOrderSaga.TimeAndMaterialJobOrderSagaData>,
        IAmInitiatedBy<RegisterTimeAndMaterialJobOrderCommand>,
        IHandleMessages<ExtendTimeAndMaterialJobOrderCommand>,
        IHandleMessages<MarkTimeAndMaterialJobOrderAsCompletedCommand>,
        IHandleMessages<LinkIncomingInvoiceToJobOrderCommand>,
        IHandleMessages<LinkOutgoingInvoiceToJobOrderCommand>
    {
        private readonly IRepository _repository;
        private readonly IEventStore _eventStore;
        private readonly IBus _bus;
        public IJobOrderNumberGenerator JobOrderNumberGenerator { get; private set; }

        public TimeAndMaterialJobOrderSaga(IBus bus, IEventStore eventStore, IRepository repository, IJobOrderNumberGenerator jobOrderNumberGenerator)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (jobOrderNumberGenerator == null)
                throw new ArgumentNullException(nameof(jobOrderNumberGenerator));

            this._repository = repository;
            this._bus = bus;
            this._eventStore = eventStore;
            JobOrderNumberGenerator = jobOrderNumberGenerator;
        }

        protected override void CorrelateMessages(ICorrelationConfig<TimeAndMaterialJobOrderSagaData> config)
        {
            config.Correlate<RegisterTimeAndMaterialJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<ExtendTimeAndMaterialJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkIncomingInvoiceToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkOutgoingInvoiceToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<MarkTimeAndMaterialJobOrderAsCompletedCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);
        }

        public Task Handle(RegisterTimeAndMaterialJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = TimeAndMaterialJobOrder.Factory.CreateNewInstance(
                JobOrderNumberGenerator,
                message.CustomerId,
                message.ManagerId,
                message.Value,
                message.Currency,
                message.DateOfStart,
                message.DateOfExpiration,
                message.JobOrderName,
                message.PurchaseOrderNumber,
                message.Description
                );
                this._repository.Save(jobOrder);
                this.Data.Id = jobOrder.Id;
            });
        }

        public Task Handle(ExtendTimeAndMaterialJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<TimeAndMaterialJobOrder>(message.JobOrderId);
                jobOrder.Extend(message.NewDateOfExpiration, message.Value);
                _repository.Save(jobOrder);
            });
        }

        public Task Handle(MarkTimeAndMaterialJobOrderAsCompletedCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<TimeAndMaterialJobOrder>(message.JobOrderId);
                jobOrder.MarkAsCompleted(message.DateOfCompletion);
                _repository.Save(jobOrder);
                this.MarkAsComplete();
            });
        }

        public Task Handle(LinkIncomingInvoiceToJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<TimeAndMaterialJobOrder>(message.JobOrderId);
                jobOrder.LinkIncomingInvoice(_eventStore, message.InvoiceId, message.DateOfLink, message.Amount);
                _repository.Save(jobOrder);
            });
        }

        public Task Handle(LinkOutgoingInvoiceToJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<TimeAndMaterialJobOrder>(message.JobOrderId);
                jobOrder.LinkOutgoingInvoice(_eventStore, message.InvoiceId, message.DateOfLink, message.Amount);
                _repository.Save(jobOrder);
            });
        }

        public class TimeAndMaterialJobOrderSagaData : SagaData
        {
            public Guid JobOrderId { get; set; }
        }
    }
}

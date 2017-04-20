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
    public sealed class FixedPriceJobOrderSaga : Saga<FixedPriceJobOrderSaga.FixedPriceJobOrderSagaData>,
        IAmInitiatedBy<RegisterFixedPriceJobOrderCommand>,
        IHandleMessages<ExtendFixedPriceJobOrderCommand>,
        IHandleMessages<LinkIncomingInvoiceToJobOrderCommand>,
        IHandleMessages<LinkOutgoingInvoiceToJobOrderCommand>,
        IHandleMessages<MarkFixedPriceJobOrderAsCompletedCommand>
    {
        private readonly IRepository _repository;
        private readonly IEventStore _eventStore;
        private readonly IBus _bus;
        public IJobOrderNumberGenerator JobOrderNumberGenerator { get; private set; }

        public FixedPriceJobOrderSaga(IBus bus, IEventStore eventStore, IRepository repository, IJobOrderNumberGenerator jobOrderNumberGenerator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            JobOrderNumberGenerator = jobOrderNumberGenerator ?? throw new ArgumentNullException(nameof(jobOrderNumberGenerator));
        }

        protected override void CorrelateMessages(ICorrelationConfig<FixedPriceJobOrderSagaData> config)
        {
            config.Correlate<RegisterFixedPriceJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<ExtendFixedPriceJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkIncomingInvoiceToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<LinkOutgoingInvoiceToJobOrderCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);

            config.Correlate<MarkFixedPriceJobOrderAsCompletedCommand>(
                message => message.JobOrderId,
                sagaData => sagaData.JobOrderId);
        }

        public Task Handle(RegisterFixedPriceJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = FixedPriceJobOrder.Factory.CreateNewInstance(
                JobOrderNumberGenerator,
                message.CustomerId,
                message.ManagerId,
                message.Price,
                message.Currency,
                message.DateOfStart,
                message.DueDate,
                message.JobOrderName,
                message.PurchaseOrderNumber,
                message.Description
                );
                this._repository.Save(jobOrder);
                this.Data.JobOrderId = jobOrder.Id;
            });
        }

        public Task Handle(ExtendFixedPriceJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<FixedPriceJobOrder>(message.JobOrderId);
                jobOrder.Extend(message.NewDueDate, message.Price);
                _repository.Save(jobOrder);
            });
        }

        public Task Handle(MarkFixedPriceJobOrderAsCompletedCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<FixedPriceJobOrder>(message.JobOrderId);
                jobOrder.MarkAsCompleted(message.DateOfCompletion);
                _repository.Save(jobOrder);
                this.MarkAsComplete();
            });
        }

        public Task Handle(LinkIncomingInvoiceToJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<FixedPriceJobOrder>(message.JobOrderId);
                jobOrder.LinkIncomingInvoice(_eventStore, message.InvoiceId, message.DateOfLink, message.Amount);
                _repository.Save(jobOrder);
            });
        }

        public Task Handle(LinkOutgoingInvoiceToJobOrderCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var jobOrder = _repository.GetById<FixedPriceJobOrder>(message.JobOrderId);
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

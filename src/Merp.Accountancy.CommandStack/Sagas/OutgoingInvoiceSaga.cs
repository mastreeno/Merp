using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using System;
using Memento.Persistence;
using Rebus.Sagas;
using Rebus.Bus;
using System.Threading.Tasks;
using Rebus.Handlers;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public class OutgoingInvoiceSaga : Saga<OutgoingInvoiceSaga.OutgoingInvoiceSagaData>,
        IAmInitiatedBy<IssueInvoiceCommand>,
        IHandleMessages<MarkOutgoingInvoiceAsPaidCommand>
    {
        private readonly IRepository _repository;
        public IOutgoingInvoiceNumberGenerator InvoiceNumberGenerator { get; private set; }

        public OutgoingInvoiceSaga(IBus bus, IEventStore eventStore, IRepository repository, IOutgoingInvoiceNumberGenerator invoiceNumberGenerator)
        {
            InvoiceNumberGenerator = invoiceNumberGenerator ?? throw new ArgumentNullException(nameof(invoiceNumberGenerator));
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override void CorrelateMessages(ICorrelationConfig<OutgoingInvoiceSagaData> config)
        {
            config.Correlate<IssueInvoiceCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
        }

        public Task Handle(IssueInvoiceCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = OutgoingInvoice.Factory.Create(
                this.InvoiceNumberGenerator,
                message.InvoiceDate,
                message.Amount,
                message.Taxes,
                message.TotalPrice,
                message.Description,
                message.PaymentTerms,
                message.PurchaseOrderNumber,
                message.Customer.Id,
                message.Customer.Name
                );
                this._repository.Save(invoice);
                this.Data.InvoiceId = invoice.Id;
            });
        }

        public Task Handle(MarkOutgoingInvoiceAsPaidCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = _repository.GetById<OutgoingInvoice>(message.InvoiceId);
                invoice.MarkAsPaid(message.PaymentDate);
                _repository.Save(invoice);
                this.MarkAsComplete();
            });
        }

        public class OutgoingInvoiceExpiredTimeout
        {
            public Guid InvoiceId { get; set; }
        }

        public class OutgoingInvoiceSagaData : SagaData
        {
            public Guid InvoiceId { get; set; }
        }
    }
}

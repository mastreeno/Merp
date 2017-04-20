using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Memento.Persistence;
using Rebus.Sagas;
using Rebus.Bus;
using System;
using System.Threading.Tasks;
using Rebus.Handlers;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public class IncomingInvoiceSaga : Saga<IncomingInvoiceSaga.IncomingInvoiceSagaData>,
        IAmInitiatedBy<RegisterIncomingInvoiceCommand>,
        IHandleMessages<MarkIncomingInvoiceAsPaidCommand>
    {
        private readonly IRepository _repository;

        public IncomingInvoiceSaga(IRepository repository)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override void CorrelateMessages(ICorrelationConfig<IncomingInvoiceSagaData> config)
        {
            config.Correlate<RegisterIncomingInvoiceCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
        }

        public Task Handle(RegisterIncomingInvoiceCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = IncomingInvoice.Factory.Create(
                message.InvoiceNumber,
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

        public Task Handle(MarkIncomingInvoiceAsPaidCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = _repository.GetById<IncomingInvoice>(message.InvoiceId);
                invoice.MarkAsPaid(message.PaymentDate);
                _repository.Save(invoice);
                this.MarkAsComplete();
            });
        }

        public class IncomingInvoiceSagaData : SagaData
        {
            public Guid InvoiceId { get; set; }
        }
    }
}

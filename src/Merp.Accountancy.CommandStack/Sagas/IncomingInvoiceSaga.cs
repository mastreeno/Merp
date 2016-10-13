using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Memento.Persistence;
using Rebus.Sagas;
using Rebus.Bus;
using System;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public class IncomingInvoiceSaga : Saga<IncomingInvoiceSaga.IncomingInvoiceSagaData>,
        IAmInitiatedBy<RegisterIncomingInvoiceCommand>
    {
        private readonly IRepository _repository;

        public IncomingInvoiceSaga(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            this._repository = repository;
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
                this.Data.Id = invoice.Id;
            });
        }

        public class IncomingInvoiceSagaData : SagaData
        {
            public Guid InvoiceId { get; set; }
        }
    }
}

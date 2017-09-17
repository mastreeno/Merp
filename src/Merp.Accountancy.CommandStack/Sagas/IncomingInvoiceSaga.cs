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
        IAmInitiatedBy<ImportIncomingInvoiceCommand>,
        IHandleMessages<MarkIncomingInvoiceAsPaidCommand>,
        IHandleMessages<MarkIncomingInvoiceAsExpiredCommand>,
        IHandleMessages<IncomingInvoiceSaga.IncomingInvoiceExpiredTimeout>
    {
        public readonly IBus Bus;
        public readonly IRepository Repository;

        public IncomingInvoiceSaga(IBus bus, IRepository repository)
        {
            this.Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override void CorrelateMessages(ICorrelationConfig<IncomingInvoiceSagaData> config)
        {
            config.Correlate<RegisterIncomingInvoiceCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<ImportIncomingInvoiceCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<MarkIncomingInvoiceAsPaidCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<MarkIncomingInvoiceAsExpiredCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<IncomingInvoiceSaga.IncomingInvoiceExpiredTimeout>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
        }

        public Task Handle(RegisterIncomingInvoiceCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = IncomingInvoice.Factory.Register(
                message.InvoiceNumber,
                message.InvoiceDate,
                message.DueDate,
                message.TaxableAmount,
                message.Taxes,
                message.TotalPrice,
                message.Description,
                message.PaymentTerms,
                message.PurchaseOrderNumber,
                message.Supplier.Id,
                message.Supplier.Name,
                message.Supplier.Address,
                message.Supplier.City,
                message.Supplier.PostalCode,
                message.Supplier.Country,
                message.Supplier.VatIndex,
                message.Supplier.NationalIdentificationNumber
                );
                this.Repository.Save(invoice);
                this.Data.InvoiceId = invoice.Id;

                if (invoice.DueDate.HasValue)
                {
                    var timeout = new IncomingInvoiceExpiredTimeout(invoice.Id);
                    Bus.Defer(invoice.DueDate.Value.Subtract(DateTime.Today), timeout);
                }
            });
        }
        public Task Handle(ImportIncomingInvoiceCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = IncomingInvoice.Factory.Import(
                    message.InvoiceId,
                    message.InvoiceNumber,
                    message.InvoiceDate,
                    message.DueDate,
                    message.TaxableAmount,
                    message.Taxes,
                    message.TotalPrice,
                    message.Description,
                    message.PaymentTerms,
                    message.PurchaseOrderNumber,
                    message.Supplier.Id,
                    message.Supplier.Name,
                    message.Supplier.Address,
                    message.Supplier.City,
                    message.Supplier.PostalCode,
                    message.Supplier.Country,
                    message.Supplier.VatIndex,
                    message.Supplier.NationalIdentificationNumber
                    );
                this.Repository.Save(invoice);
                this.Data.InvoiceId = invoice.Id;
            });
        }

        public Task Handle(MarkIncomingInvoiceAsPaidCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = Repository.GetById<IncomingInvoice>(message.InvoiceId);
                invoice.MarkAsPaid(message.PaymentDate);
                Repository.Save(invoice);
                this.MarkAsComplete();
            });
        }

        public Task Handle(MarkIncomingInvoiceAsExpiredCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = Repository.GetById<IncomingInvoice>(message.InvoiceId);
                if (!invoice.PaymentDate.HasValue)
                    invoice.MarkAsExpired();
            });
        }

        public Task Handle(IncomingInvoiceExpiredTimeout message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = Repository.GetById<IncomingInvoice>(message.InvoiceId);
                if (!invoice.PaymentDate.HasValue)
                {
                    var cmd = new MarkIncomingInvoiceAsExpiredCommand(message.InvoiceId);
                    Bus.Send(cmd);
                }
            });
        }

        public class IncomingInvoiceSagaData : SagaData
        {
            public Guid InvoiceId { get; set; }
        }

        public class IncomingInvoiceExpiredTimeout
        {
            public Guid InvoiceId { get; private set; }

            public IncomingInvoiceExpiredTimeout(Guid invoiceId)
            {
                InvoiceId = invoiceId;
            }
        }
    }
}

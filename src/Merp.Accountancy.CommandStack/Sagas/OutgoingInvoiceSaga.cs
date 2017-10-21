using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using System;
using MementoFX.Persistence;
using Rebus.Sagas;
using Rebus.Bus;
using System.Threading.Tasks;
using Rebus.Handlers;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public class OutgoingInvoiceSaga : Saga<OutgoingInvoiceSaga.OutgoingInvoiceSagaData>,
        IAmInitiatedBy<IssueInvoiceCommand>,
        IAmInitiatedBy<ImportOutgoingInvoiceCommand>,
        IHandleMessages<MarkOutgoingInvoiceAsPaidCommand>,
        IHandleMessages<MarkOutgoingInvoiceAsOverdueCommand>,
        IHandleMessages<OutgoingInvoiceSaga.OutgoingInvoiceExpiredTimeout>
    {
        public readonly IBus Bus;
        public readonly IRepository Repository;
        public IOutgoingInvoiceNumberGenerator InvoiceNumberGenerator { get; private set; }       

        public OutgoingInvoiceSaga(IBus bus, IRepository repository, IOutgoingInvoiceNumberGenerator invoiceNumberGenerator)
        {
            InvoiceNumberGenerator = invoiceNumberGenerator ?? throw new ArgumentNullException(nameof(invoiceNumberGenerator));
            this.Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override void CorrelateMessages(ICorrelationConfig<OutgoingInvoiceSagaData> config)
        {
            config.Correlate<IssueInvoiceCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<ImportOutgoingInvoiceCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<MarkOutgoingInvoiceAsPaidCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<MarkOutgoingInvoiceAsOverdueCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<OutgoingInvoiceSaga.OutgoingInvoiceExpiredTimeout>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
        }

        public Task Handle(IssueInvoiceCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = OutgoingInvoice.Factory.Issue(
                    this.InvoiceNumberGenerator,
                    message.InvoiceDate,
                    message.Currency,
                    message.TaxableAmount,
                    message.Taxes,
                    message.TotalPrice,
                    message.Description,
                    message.PaymentTerms,
                    message.PurchaseOrderNumber,
                    message.Customer.Id,
                    message.Customer.Name,
                    message.Customer.StreetName,
                    message.Customer.City,
                    message.Customer.PostalCode,
                    message.Customer.Country,
                    message.Customer.VatIndex,
                    message.Customer.NationalIdentificationNumber,
                    message.Supplier.Name,
                    message.Supplier.StreetName,
                    message.Supplier.City,
                    message.Supplier.PostalCode,
                    message.Supplier.Country,
                    message.Supplier.VatIndex,
                    message.Supplier.NationalIdentificationNumber
                );
                this.Repository.Save(invoice);
                this.Data.InvoiceId = invoice.Id;
                
                if(invoice.DueDate.HasValue)
                {
                    var timeout = new OutgoingInvoiceExpiredTimeout(invoice.Id);
                    Bus.Defer(invoice.DueDate.Value.Subtract(DateTime.Today), timeout);
                }    
            });
        }

        public Task Handle(MarkOutgoingInvoiceAsPaidCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = Repository.GetById<OutgoingInvoice>(message.InvoiceId);
                invoice.MarkAsPaid(message.PaymentDate);
                Repository.Save(invoice);
                this.MarkAsComplete();
            });
        }

        public Task Handle(ImportOutgoingInvoiceCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = OutgoingInvoice.Factory.Import(
                    message.InvoiceId,
                    message.InvoiceNumber,
                    message.InvoiceDate,
                    message.DueDate,
                    message.Currency,
                    message.TaxableAmount,
                    message.Taxes,
                    message.TotalPrice,
                    message.Description,
                    message.PaymentTerms,
                    message.PurchaseOrderNumber,
                    message.Customer.Id,
                    message.Customer.Name,
                    message.Customer.StreetName,
                    message.Customer.City,
                    message.Customer.PostalCode,
                    message.Customer.Country,
                    message.Customer.VatIndex,
                    message.Customer.NationalIdentificationNumber,
                    message.Supplier.Name,
                    message.Supplier.StreetName,
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

        public Task Handle(MarkOutgoingInvoiceAsOverdueCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = Repository.GetById<OutgoingInvoice>(message.InvoiceId);
                if (!invoice.PaymentDate.HasValue)
                    invoice.MarkAsOverdue();
            });
        }

        public Task Handle(OutgoingInvoiceExpiredTimeout message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = Repository.GetById<OutgoingInvoice>(message.InvoiceId);
                if (!invoice.PaymentDate.HasValue)
                {
                    var cmd = new MarkOutgoingInvoiceAsOverdueCommand(message.InvoiceId);
                    Bus.Send(cmd);
                }
            });
        }

        public class OutgoingInvoiceSagaData : SagaData
        {
            public Guid InvoiceId { get; set; }
        }

        public class OutgoingInvoiceExpiredTimeout
        {
            public Guid InvoiceId { get; private set; }

            public OutgoingInvoiceExpiredTimeout(Guid invoiceId)
            {
                InvoiceId = invoiceId;
            }
        }
    }
}

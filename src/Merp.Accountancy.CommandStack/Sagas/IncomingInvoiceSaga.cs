using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using MementoFX.Persistence;
using Rebus.Sagas;
using Rebus.Bus;
using System;
using System.Threading.Tasks;
using Rebus.Handlers;
using System.Linq;
using System.Collections.Generic;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public class IncomingInvoiceSaga : Saga<IncomingInvoiceSaga.IncomingInvoiceSagaData>,
        IAmInitiatedBy<RegisterIncomingInvoiceCommand>,
        IAmInitiatedBy<ImportIncomingInvoiceCommand>,
        IHandleMessages<MarkIncomingInvoiceAsPaidCommand>,
        IHandleMessages<MarkIncomingInvoiceAsOverdueCommand>,
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
            config.Correlate<MarkIncomingInvoiceAsOverdueCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<IncomingInvoiceSaga.IncomingInvoiceExpiredTimeout>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
        }

        public async Task Handle(RegisterIncomingInvoiceCommand message)
        {
            var invoiceLineItems = new Invoice.InvoiceLineItem[0];
            if (message.LineItems != null)
            {
                invoiceLineItems = message.LineItems
                    .Select(i => new Invoice.InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat))
                    .ToArray();
            }

            var invoicePricesByVat = new Invoice.InvoicePriceByVat[0];
            if (message.PricesByVat != null)
            {
                invoicePricesByVat = message.PricesByVat
                    .Select(p => new Invoice.InvoicePriceByVat(p.TaxableAmount, p.VatRate, p.VatAmount, p.TotalPrice))
                    .ToArray();
            }

            var nonTaxableItems = new Invoice.NonTaxableItem[0];
            if (message.NonTaxableItems != null)
            {
                nonTaxableItems = message.NonTaxableItems
                    .Select(t => new Invoice.NonTaxableItem(t.Description, t.Amount))
                    .ToArray();
            }

            var invoice = IncomingInvoice.Factory.Register(
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
            message.Customer.Address,
            message.Customer.City,
            message.Customer.PostalCode,
            message.Customer.Country,
            message.Customer.VatIndex,
            message.Customer.NationalIdentificationNumber,
            message.Supplier.Id,
            message.Supplier.Name,
            message.Supplier.Address,
            message.Supplier.City,
            message.Supplier.PostalCode,
            message.Supplier.Country,
            message.Supplier.VatIndex,
            message.Supplier.NationalIdentificationNumber,
            invoiceLineItems,
            message.PricesAreVatIncluded,
            invoicePricesByVat,
            nonTaxableItems,
            message.UserId
            );
            this.Repository.Save(invoice);
            this.Data.InvoiceId = invoice.Id;

            if (invoice.DueDate.HasValue)
            {
                var timeout = new IncomingInvoiceExpiredTimeout(invoice.Id, message.UserId);
                await Bus.Defer(invoice.DueDate.Value.Subtract(DateTime.Today), timeout);
            }
        }
        public async Task Handle(ImportIncomingInvoiceCommand message)
        {
            var invoice = IncomingInvoice.Factory.Import(
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
                message.Customer.Address,
                message.Customer.City,
                message.Customer.PostalCode,
                message.Customer.Country,
                message.Customer.VatIndex,
                message.Customer.NationalIdentificationNumber,
                message.Supplier.Id,
                message.Supplier.Name,
                message.Supplier.Address,
                message.Supplier.City,
                message.Supplier.PostalCode,
                message.Supplier.Country,
                message.Supplier.VatIndex,
                message.Supplier.NationalIdentificationNumber
                );
            await this.Repository.SaveAsync(invoice);
            this.Data.InvoiceId = invoice.Id;
        }

        public async Task Handle(MarkIncomingInvoiceAsPaidCommand message)
        {
            var invoice = Repository.GetById<IncomingInvoice>(message.InvoiceId);
            invoice.MarkAsPaid(message.PaymentDate, message.UserId);
            await Repository.SaveAsync(invoice);
            this.MarkAsComplete();
        }

        public Task Handle(MarkIncomingInvoiceAsOverdueCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var invoice = Repository.GetById<IncomingInvoice>(message.InvoiceId);
                if (!invoice.PaymentDate.HasValue)
                    invoice.MarkAsOverdue(message.UserId);
            });
        }

        public async Task Handle(IncomingInvoiceExpiredTimeout message)
        {
            var invoice = Repository.GetById<IncomingInvoice>(message.InvoiceId);
            if (!invoice.PaymentDate.HasValue)
            {
                var cmd = new MarkIncomingInvoiceAsOverdueCommand(message.UserId, message.InvoiceId);
                await Bus.Send(cmd);
            }
        }

        public class IncomingInvoiceSagaData : SagaData
        {
            public Guid InvoiceId { get; set; }
        }

        public class IncomingInvoiceExpiredTimeout
        {
            public Guid InvoiceId { get; private set; }

            public Guid UserId { get; private set; }

            public IncomingInvoiceExpiredTimeout(Guid invoiceId, Guid userId)
            {
                InvoiceId = invoiceId;
                UserId = userId;
            }
        }
    }
}

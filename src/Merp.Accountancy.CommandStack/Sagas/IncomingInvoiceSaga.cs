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
        IHandleMessages<IncomingInvoiceSaga.IncomingInvoiceOverdueTimeout>
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
            config.Correlate<IncomingInvoiceSaga.IncomingInvoiceOverdueTimeout>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
        }

        public async Task Handle(RegisterIncomingInvoiceCommand message)
        {
            var invoiceLineItems = new Invoice.InvoiceLineItem[0];
            if (message.LineItems != null)
            {
                invoiceLineItems = message.LineItems
                    .Select(i => new Invoice.InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat, i.VatDescription))
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
            message.TotalToPay,
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
            message.ProvidenceFundDescription,
            message.ProvidenceFundRate,
            message.ProvidenceFundAmount,
            message.WithholdingTaxDescription,
            message.WithholdingTaxRate,
            message.WithholdingTaxTaxableAmountRate,
            message.WithholdingTaxAmount,
            message.UserId
            );
            this.Repository.Save(invoice);
            this.Data.InvoiceId = invoice.Id;

            if (invoice.DueDate.HasValue)
            {
                var timeout = new IncomingInvoiceOverdueTimeout(invoice.Id, message.UserId);
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
                message.TotalToPay,
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
                message.LineItems
                    .Select(i => new IncomingInvoice.InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat, i.VatDescription))
                    .ToArray(),
                false,
                message.PricesByVat
                    .Select(i => new IncomingInvoice.InvoicePriceByVat(i.TaxableAmount, i.VatRate, i.VatAmount, i.TotalPrice))
                    .ToArray(),
                message.NonTaxableItems
                    .Select(i => new IncomingInvoice.NonTaxableItem(i.Description, i.Amount))
                    .ToArray(),
                message.ProvidenceFundDescription,
                message.ProvidenceFundRate,
                message.ProvidenceFundAmount,
                message.WithholdingTaxDescription,
                message.WithholdingTaxRate,
                message.WithholdingTaxTaxableAmountRate,
                message.WithholdingTaxAmount,
                message.UserId
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

        public async Task Handle(MarkIncomingInvoiceAsOverdueCommand message)
        {
            var invoice = Repository.GetById<IncomingInvoice>(message.InvoiceId);
            if (!invoice.PaymentDate.HasValue)
                invoice.MarkAsOverdue(message.UserId);
            await Repository.SaveAsync(invoice);
        }

        public async Task Handle(IncomingInvoiceOverdueTimeout message)
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

        public class IncomingInvoiceOverdueTimeout
        {
            public Guid InvoiceId { get; private set; }

            public Guid UserId { get; private set; }

            public IncomingInvoiceOverdueTimeout(Guid invoiceId, Guid userId)
            {
                InvoiceId = invoiceId;
                UserId = userId;
            }
        }
    }
}

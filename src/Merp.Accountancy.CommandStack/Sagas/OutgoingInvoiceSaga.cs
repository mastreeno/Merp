using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using System;
using MementoFX.Persistence;
using Rebus.Sagas;
using Rebus.Bus;
using System.Threading.Tasks;
using Rebus.Handlers;
using System.Linq;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public class OutgoingInvoiceSaga : Saga<OutgoingInvoiceSaga.OutgoingInvoiceSagaData>,
        IAmInitiatedBy<IssueInvoiceCommand>,
        IAmInitiatedBy<ImportOutgoingInvoiceCommand>,
        IAmInitiatedBy<RegisterOutgoingInvoiceCommand>,
        IHandleMessages<MarkOutgoingInvoiceAsPaidCommand>,
        IHandleMessages<MarkOutgoingInvoiceAsOverdueCommand>,
        IHandleMessages<OutgoingInvoiceSaga.OutgoingInvoiceOverdueTimeout>
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
            config.Correlate<RegisterOutgoingInvoiceCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<MarkOutgoingInvoiceAsPaidCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<MarkOutgoingInvoiceAsOverdueCommand>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
            config.Correlate<OutgoingInvoiceSaga.OutgoingInvoiceOverdueTimeout>(
                message => message.InvoiceId,
                sagaData => sagaData.InvoiceId);
        }

        public async Task Handle(IssueInvoiceCommand message)
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

            var invoice = OutgoingInvoice.Factory.Issue(
                this.InvoiceNumberGenerator,
                message.InvoiceDate,
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
                
            if(invoice.DueDate.HasValue)
            {
                var timeout = new OutgoingInvoiceOverdueTimeout(invoice.Id, message.UserId);
                await Bus.Defer(invoice.DueDate.Value.Subtract(DateTime.Today), timeout);
            }    
        }

        public async Task Handle(MarkOutgoingInvoiceAsPaidCommand message)
        {
            var invoice = Repository.GetById<OutgoingInvoice>(message.InvoiceId);
            invoice.MarkAsPaid(message.PaymentDate, message.UserId);
            await Repository.SaveAsync(invoice);
            this.MarkAsComplete();
        }

        public async Task Handle(ImportOutgoingInvoiceCommand message)
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
                message.TotalToPay,
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
                message.Supplier.NationalIdentificationNumber,
                message.LineItems
                    .Select(i => new OutgoingInvoice.InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat, i.VatDescription))
                    .ToArray(),
                false,
                message.PricesByVat
                    .Select(i => new OutgoingInvoice.InvoicePriceByVat(i.TaxableAmount, i.VatRate, i.VatAmount, i.TotalPrice))
                    .ToArray(),
                message.NonTaxableItems
                    .Select(i => new OutgoingInvoice.NonTaxableItem(i.Description, i.Amount))
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

        public async Task Handle(RegisterOutgoingInvoiceCommand message)
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

            var invoice = OutgoingInvoice.Factory.Register(
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
                var timeout = new OutgoingInvoiceOverdueTimeout(invoice.Id, message.UserId);
                await Bus.Defer(invoice.DueDate.Value.Subtract(DateTime.Today), timeout);
            }
        }

        public async Task Handle(MarkOutgoingInvoiceAsOverdueCommand message)
        {
            var invoice = Repository.GetById<OutgoingInvoice>(message.InvoiceId);
            if (!invoice.PaymentDate.HasValue)
                invoice.MarkAsOverdue(message.UserId);
            await Repository.SaveAsync(invoice);
        }

        public async Task Handle(OutgoingInvoiceOverdueTimeout message)
        {
            var invoice = Repository.GetById<OutgoingInvoice>(message.InvoiceId);
            if (!invoice.PaymentDate.HasValue)
            {
                var cmd = new MarkOutgoingInvoiceAsOverdueCommand(message.UserId, message.InvoiceId);
                await Bus.Send(cmd);
            }
        }

        public class OutgoingInvoiceSagaData : SagaData
        {
            public Guid InvoiceId { get; set; }
        }

        public class OutgoingInvoiceOverdueTimeout
        {
            public Guid InvoiceId { get; private set; }

            public Guid UserId { get; private set; }

            public OutgoingInvoiceOverdueTimeout(Guid invoiceId, Guid userId)
            {
                InvoiceId = invoiceId;
            }
        }
    }
}

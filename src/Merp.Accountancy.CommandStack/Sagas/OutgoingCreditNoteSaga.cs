using MementoFX.Persistence;
using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using Rebus.Bus;
using Rebus.Sagas;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public class OutgoingCreditNoteSaga : Saga<OutgoingCreditNoteSaga.OutgoingCreditNoteSagaData>,
        IAmInitiatedBy<IssueCreditNoteCommand>,
        IAmInitiatedBy<RegisterOutgoingCreditNoteCommand>
    {
        public readonly IBus Bus;
        public readonly IRepository Repository;

        public IOutgoingCreditNoteNumberGenerator OutgoingCreditNoteNumberGenerator { get; private set; }

        public OutgoingCreditNoteSaga(IBus bus, IRepository repository, IOutgoingCreditNoteNumberGenerator outgoingCreditNoteNumberGenerator)
        {
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            OutgoingCreditNoteNumberGenerator = outgoingCreditNoteNumberGenerator ?? throw new ArgumentNullException(nameof(outgoingCreditNoteNumberGenerator));
        }

        protected override void CorrelateMessages(ICorrelationConfig<OutgoingCreditNoteSagaData> config)
        {
            config.Correlate<IssueCreditNoteCommand>(
                message => message.CreditNoteId,
                sagaData => sagaData.CreditNoteId);

            config.Correlate<RegisterOutgoingCreditNoteCommand>(
                message => message.CreditNoteId,
                sagaData => sagaData.CreditNoteId);
        }

        public async Task Handle(IssueCreditNoteCommand message)
        {
            var lineItems = new Invoice.InvoiceLineItem[0];
            if (message.LineItems != null)
            {
                lineItems = message.LineItems
                    .Select(i => new Invoice.InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat, i.VatDescription))
                    .ToArray();
            }

            var pricesByVat = new Invoice.InvoicePriceByVat[0];
            if (message.PricesByVat != null)
            {
                pricesByVat = message.PricesByVat
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

            var creditNote = OutgoingCreditNote.Factory.Issue(
                OutgoingCreditNoteNumberGenerator,
                message.CreditNoteDate,
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
                lineItems,
                message.PricesAreVatIncluded,
                pricesByVat,
                nonTaxableItems,
                message.ProvidenceFundDescription,
                message.ProvidenceFundRate,
                message.ProvidenceFundAmount,
                message.WithholdingTaxDescription,
                message.WithholdingTaxRate,
                message.WithholdingTaxTaxableAmountRate,
                message.WithholdingTaxAmount,
                message.UserId);

            await Repository.SaveAsync(creditNote);
            Data.CreditNoteId = creditNote.Id;
        }

        public async Task Handle(RegisterOutgoingCreditNoteCommand message)
        {
            var lineItems = new Invoice.InvoiceLineItem[0];
            if (message.LineItems != null)
            {
                lineItems = message.LineItems
                    .Select(i => new Invoice.InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat, i.VatDescription))
                    .ToArray();
            }

            var pricesByVat = new Invoice.InvoicePriceByVat[0];
            if (message.PricesByVat != null)
            {
                pricesByVat = message.PricesByVat
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

            var creditNote = OutgoingCreditNote.Factory.Register(
                message.CreditNoteNumber,
                message.CreditNoteDate,
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
                lineItems,
                message.PricesAreVatIncluded,
                pricesByVat,
                nonTaxableItems,
                message.ProvidenceFundDescription,
                message.ProvidenceFundRate,
                message.ProvidenceFundAmount,
                message.WithholdingTaxDescription,
                message.WithholdingTaxRate,
                message.WithholdingTaxTaxableAmountRate,
                message.WithholdingTaxAmount,
                message.UserId);

            await Repository.SaveAsync(creditNote);
            Data.CreditNoteId = creditNote.Id;
        }

        public class OutgoingCreditNoteSagaData : SagaData
        {
            public Guid CreditNoteId { get; set; }
        }
    }
}

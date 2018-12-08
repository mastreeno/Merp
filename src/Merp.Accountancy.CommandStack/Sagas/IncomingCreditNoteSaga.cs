using MementoFX.Persistence;
using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Rebus.Bus;
using Rebus.Sagas;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Sagas
{
    public class IncomingCreditNoteSaga : Saga<IncomingCreditNoteSaga.IncomingCreditNoteSagaData>,
        IAmInitiatedBy<RegisterIncomingCreditNoteCommand>
    {
        public readonly IBus Bus;
        public readonly IRepository Repository;

        public IncomingCreditNoteSaga(IBus bus, IRepository repository)
        {
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override void CorrelateMessages(ICorrelationConfig<IncomingCreditNoteSagaData> config)
        {
            config.Correlate<RegisterIncomingCreditNoteCommand>(
                message => message.CreditNoteId,
                sagaData => sagaData.CreditNoteId);
        }

        public async Task Handle(RegisterIncomingCreditNoteCommand message)
        {
            var lineItems = new Invoice.InvoiceLineItem[0];
            if (message.LineItems != null)
            {
                lineItems = message.LineItems
                    .Select(i => new Invoice.InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat))
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

            var creditNote = IncomingCreditNote.Factory.Register(
                message.CreditNoteNumber,
                message.CreditNoteDate,
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
                lineItems,
                message.PricesAreVatIncluded,
                pricesByVat,
                nonTaxableItems,
                message.UserId);

            await this.Repository.SaveAsync(creditNote);
            this.Data.CreditNoteId = creditNote.Id;
        }

        public class IncomingCreditNoteSagaData : SagaData
        {
            public Guid CreditNoteId { get; set; }
        }
    }
}

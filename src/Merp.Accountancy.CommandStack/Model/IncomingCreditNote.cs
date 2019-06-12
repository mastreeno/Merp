using MementoFX.Domain;
using Merp.Accountancy.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Merp.Accountancy.CommandStack.Model
{
    public class IncomingCreditNote : Invoice,
        IApplyEvent<IncomingCreditNoteRegisteredEvent>
    {
        public PartyInfo Supplier { get; protected set; }

        protected IncomingCreditNote() { }

        public void ApplyEvent([AggregateId(nameof(IncomingCreditNoteRegisteredEvent.CreditNoteId))] IncomingCreditNoteRegisteredEvent evt)
        {
            Id = evt.CreditNoteId;
            IsOverdue = false;
            Number = evt.CreditNoteNumber;
            Date = evt.CreditNoteDate;
            Currency = evt.Currency;
            Amount = evt.TaxableAmount;
            Taxes = evt.Taxes;
            TotalPrice = evt.TotalPrice;
            TotalToPay = evt.TotalToPay;
            Description = evt.Description;
            PaymentTerms = evt.PaymentTerms;
            PurchaseOrderNumber = evt.PurchaseOrderNumber;
            Supplier = new PartyInfo(evt.Supplier.Id, evt.Supplier.Name, evt.Supplier.StreetName, evt.Supplier.City, evt.Supplier.PostalCode, evt.Supplier.Country, evt.Supplier.VatIndex, evt.Supplier.NationalIdentificationNumber);

            if (evt.LineItems != null)
            {
                InvoiceLineItems = evt.LineItems
                    .Select(i => new InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat, i.VatDescription))
                    .ToArray();
            }

            if (evt.PricesByVat != null)
            {
                InvoicePricesByVat = evt.PricesByVat
                    .Select(p => new InvoicePriceByVat(p.TaxableAmount, p.VatRate, p.VatAmount, p.TotalPrice))
                    .ToArray();
            }

            if (evt.NonTaxableItems != null)
            {
                NonTaxableItems = evt.NonTaxableItems
                    .Select(t => new NonTaxableItem(t.Description, t.Amount))
                    .ToArray();
            }

            if (!string.IsNullOrWhiteSpace(evt.ProvidenceFundDescription) && evt.ProvidenceFundRate.HasValue && evt.ProvidenceFundAmount.HasValue)
            {
                ProvidenceFund = new InvoiceProvidenceFund(evt.ProvidenceFundDescription, evt.ProvidenceFundRate.Value, evt.ProvidenceFundAmount.Value);
            }

            if (!string.IsNullOrWhiteSpace(evt.WithholdingTaxDescription) && evt.WithholdingTaxRate.HasValue && evt.WithholdingTaxTaxableAmountRate.HasValue && evt.WithholdingTaxAmount.HasValue)
            {
                WithholdingTax = new InvoiceWithholdingTax(
                    evt.WithholdingTaxDescription,
                    evt.WithholdingTaxRate.Value,
                    evt.WithholdingTaxTaxableAmountRate.Value,
                    evt.WithholdingTaxAmount.Value);
            }

            PricesAreVatIncluded = evt.PricesAreVatIncluded;
        }

        public static class Factory
        {
            public static IncomingCreditNote Register(string creditNoteNumber, DateTime creditNoteDate, string currency, decimal amount, decimal taxes, decimal totalPrice, decimal totalToPay, string description, string paymentTerms, string purchaseOrderNumber,
            Guid customerId, string customerName, string customerAddress, string customerCity, string customerPostalCode, string customerCountry, string customerVatIndex, string customerNationalIdentificationNumber,
            Guid supplierId, string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationNumber, IEnumerable<InvoiceLineItem> lineItems, bool pricesAreVatIncluded, IEnumerable<InvoicePriceByVat> pricesByVat, IEnumerable<NonTaxableItem> nonTaxableItems,
            string providenceFundDescription, decimal? providenceFundRate, decimal? providenceFundAmount, string withholdingTaxDescription, decimal? withholdingTaxRate, decimal? withholdingTaxTaxableAmountRate, decimal? withholdingTaxAmount, Guid userId)
            {
                if (string.IsNullOrWhiteSpace(creditNoteNumber))
                {
                    throw new ArgumentException("value cannot be empty", nameof(creditNoteNumber));
                }

                if (lineItems == null)
                {
                    throw new ArgumentNullException(nameof(lineItems));
                }

                if (pricesByVat == null)
                {
                    throw new ArgumentNullException(nameof(pricesByVat));
                }

                var _lineItems = new IncomingCreditNoteRegisteredEvent.LineItem[0];
                if (lineItems != null && lineItems.Count() > 0)
                {
                    _lineItems = lineItems.Select(i => new IncomingCreditNoteRegisteredEvent.LineItem(
                        i.Code,
                        i.Description,
                        i.Quantity,
                        i.UnitPrice,
                        i.TotalPrice,
                        i.Vat,
                        i.VatDescription)).ToArray();
                }

                var _pricesByVat = new IncomingCreditNoteRegisteredEvent.PriceByVat[0];
                if (pricesByVat != null && pricesByVat.Count() > 0)
                {
                    _pricesByVat = pricesByVat.Select(p => new IncomingCreditNoteRegisteredEvent.PriceByVat(
                        p.TaxableAmount,
                        p.VatRate,
                        p.VatAmount,
                        p.TotalPrice)).ToArray();
                }

                var _nonTaxableItems = new IncomingCreditNoteRegisteredEvent.NonTaxableItem[0];
                if (nonTaxableItems != null && nonTaxableItems.Count() > 0)
                {
                    _nonTaxableItems = nonTaxableItems.Select(t => new IncomingCreditNoteRegisteredEvent.NonTaxableItem(t.Description, t.Amount)).ToArray();
                }

                var @event = new IncomingCreditNoteRegisteredEvent(
                        Guid.NewGuid(),
                        creditNoteNumber,
                        creditNoteDate,
                        currency,
                        amount,
                        taxes,
                        totalPrice,
                        totalToPay,
                        description,
                        paymentTerms,
                        purchaseOrderNumber,
                        customerId,
                        customerName,
                        customerAddress,
                        customerCity,
                        customerPostalCode,
                        customerCountry,
                        customerVatIndex,
                        customerNationalIdentificationNumber,
                        supplierId,
                        supplierName,
                        supplierAddress,
                        supplierCity,
                        supplierPostalCode,
                        supplierCountry,
                        supplierVatIndex,
                        supplierNationalIdentificationNumber,
                        _lineItems,
                        pricesAreVatIncluded,
                        _pricesByVat,
                        _nonTaxableItems,
                        providenceFundDescription,
                        providenceFundRate,
                        providenceFundAmount,
                        withholdingTaxDescription,
                        withholdingTaxRate,
                        withholdingTaxTaxableAmountRate,
                        withholdingTaxAmount,
                        userId
                    );

                var creditNote = new IncomingCreditNote();
                creditNote.RaiseEvent(@event);

                return creditNote;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using MementoFX.Domain;
using Merp.Accountancy.CommandStack.Events;

namespace Merp.Accountancy.CommandStack.Model
{
    public class IncomingInvoice : Invoice,
        IApplyEvent<IncomingInvoiceRegisteredEvent>,
        IApplyEvent<IncomingInvoiceGotOverdueEvent>
    {
        public PartyInfo Supplier { get; protected set; }

        protected IncomingInvoice()
        {

        }

        public void ApplyEvent([AggregateId(nameof(IncomingInvoiceRegisteredEvent.InvoiceId))] IncomingInvoiceRegisteredEvent evt)
        {
            Id = evt.InvoiceId;
            IsOverdue = false;
            Number = evt.InvoiceNumber;
            Date = evt.InvoiceDate;
            DueDate = evt.DueDate;
            Currency = evt.Currency;
            Amount = evt.TaxableAmount;
            Taxes = evt.Taxes;
            TotalPrice = evt.TotalPrice;
            Description = evt.Description;
            PaymentTerms = evt.PaymentTerms;
            PurchaseOrderNumber = evt.PurchaseOrderNumber;
            Supplier = new PartyInfo(evt.Supplier.Id, evt.Supplier.Name, evt.Supplier.StreetName, evt.Supplier.City, evt.Supplier.PostalCode, evt.Supplier.Country, evt.Supplier.VatIndex, evt.Supplier.NationalIdentificationNumber);

            if (evt.LineItems != null)
            {
                InvoiceLineItems = evt.LineItems
                    .Select(i => new InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat))
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

            PricesAreVatIncluded = evt.PricesAreVatIncluded;
        }

        public void ApplyEvent([AggregateId(nameof(IncomingInvoicePaidEvent.InvoiceId))] IncomingInvoicePaidEvent evt)
        {
            PaymentDate = evt.PaymentDate;
        }

        public void ApplyEvent([AggregateId(nameof(IncomingInvoiceGotOverdueEvent.InvoiceId))] IncomingInvoiceGotOverdueEvent evt)
        {
            IsOverdue = true;
        }

        public void MarkAsOverdue(Guid userId)
        {
            if (!DueDate.HasValue)
                throw new InvalidOperationException("An invoice must have a due date for it to be marked as expired.");

            var evt = new IncomingInvoiceGotOverdueEvent(this.Id, DueDate.Value, userId);
            RaiseEvent(evt);
        }

        public void MarkAsPaid(DateTime paymentDate, Guid userId)
        {
            var evt = new IncomingInvoicePaidEvent(this.Id, paymentDate, userId);
            RaiseEvent(evt);
        }

        public static class Factory
        {
            public static IncomingInvoice Register(string invoiceNumber, DateTime invoiceDate, DateTime? dueDate, string currency, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber,
            Guid customerId, string customerName, string customerAddress, string customerCity, string customerPostalCode, string customerCountry, string customerVatIndex, string customerNationalIdentificationNumber,
            Guid supplierId, string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationNumber, IEnumerable<InvoiceLineItem> lineItems, bool pricesAreVatIncluded, IEnumerable<InvoicePriceByVat> pricesByVat, IEnumerable<NonTaxableItem> nonTaxableItems, Guid userId)
            {
                if (string.IsNullOrWhiteSpace(invoiceNumber))
                {
                    throw new ArgumentException("value cannot be empty", nameof(invoiceNumber));
                }

                if (lineItems == null)
                {
                    throw new ArgumentNullException(nameof(lineItems));
                }

                if (pricesByVat == null)
                {
                    throw new ArgumentNullException(nameof(pricesByVat));
                }

                var _invoiceLineItems = new IncomingInvoiceRegisteredEvent.InvoiceLineItem[0];
                if (lineItems != null && lineItems.Count() > 0)
                {
                    _invoiceLineItems = lineItems.Select(i => new IncomingInvoiceRegisteredEvent.InvoiceLineItem(
                        i.Code,
                        i.Description,
                        i.Quantity,
                        i.UnitPrice,
                        i.TotalPrice,
                        i.Vat)).ToArray();
                }

                var _invoicePricesByVat = new IncomingInvoiceRegisteredEvent.InvoicePriceByVat[0];
                if (pricesByVat != null && pricesByVat.Count() > 0)
                {
                    _invoicePricesByVat = pricesByVat.Select(p => new IncomingInvoiceRegisteredEvent.InvoicePriceByVat(
                        p.TaxableAmount,
                        p.VatRate,
                        p.VatAmount,
                        p.TotalPrice)).ToArray();
                }

                var _nonTaxableItems = new IncomingInvoiceRegisteredEvent.NonTaxableItem[0];
                if (nonTaxableItems != null && nonTaxableItems.Count() > 0)
                {
                    _nonTaxableItems = nonTaxableItems.Select(t => new IncomingInvoiceRegisteredEvent.NonTaxableItem(t.Description, t.Amount)).ToArray();
                }

                var @event = new IncomingInvoiceRegisteredEvent(
                        Guid.NewGuid(),
                        invoiceNumber,
                        invoiceDate,
                        dueDate,
                        currency,
                        amount,
                        taxes,
                        totalPrice,
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
                        _invoiceLineItems,
                        pricesAreVatIncluded,
                        _invoicePricesByVat,
                        _nonTaxableItems,
                        userId
                    );
                var invoice = new IncomingInvoice();
                invoice.RaiseEvent(@event);
                return invoice;
            }

            public static IncomingInvoice Import(Guid invoiceId, string invoiceNumber, DateTime invoiceDate, DateTime? dueDate, string currency, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber,
             Guid customerId, string customerName, string customerAddress, string customerCity, string customerPostalCode, string customerCountry, string customerVatIndex, string customerNationalIdentificationNumber,
             Guid supplierId, string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationNumber)
            {
                var @event = new IncomingInvoiceRegisteredEvent(
                        invoiceId,
                        invoiceNumber,
                        invoiceDate,
                        dueDate,
                        currency,
                        amount,
                        taxes,
                        totalPrice,
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
                        null, 
                        false,
                        null,
                        null,
                        Guid.Empty
                    );
                var invoice = new IncomingInvoice();
                invoice.RaiseEvent(@event);
                return invoice;
            }
        }
    }
}

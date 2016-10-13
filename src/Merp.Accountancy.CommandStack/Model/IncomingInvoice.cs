using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;
using Merp.Accountancy.CommandStack.Events;

namespace Merp.Accountancy.CommandStack.Model
{
    public class IncomingInvoice : Invoice,
        IApplyEvent<IncomingInvoiceRegisteredEvent>
    {
        public PartyInfo Supplier { get; protected set; }

        protected IncomingInvoice()
        {

        }

        public void ApplyEvent([AggregateId(nameof(IncomingInvoiceRegisteredEvent.InvoiceId))] IncomingInvoiceRegisteredEvent evt)
        {
            Id = evt.InvoiceId;
            Number = evt.InvoiceNumber;
            Date = evt.InvoiceDate;
            Amount = evt.Amount;
            Taxes = evt.Taxes;
            TotalPrice = evt.TotalPrice;
            Description = evt.Description;
            PaymentTerms = evt.PaymentTerms;
            PurchaseOrderNumber = evt.PurchaseOrderNumber;
            Supplier = new PartyInfo(evt.Supplier.Id, evt.Supplier.Name, evt.Supplier.StreetName, evt.Supplier.City, evt.Supplier.PostalCode, evt.Supplier.Country, evt.Supplier.VatIndex, evt.Supplier.NationalIdentificationNumber);
        }

        public static class Factory
        {
            public static IncomingInvoice Create(string invoiceNumber, DateTime invoiceDate, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, Guid supplierId, string supplierName)
            {
                var @event = new IncomingInvoiceRegisteredEvent(
                    Guid.NewGuid(),
                    invoiceNumber,
                    invoiceDate,
                    amount,
                    taxes,
                    totalPrice,
                    description,
                    paymentTerms,
                    purchaseOrderNumber,
                    supplierId,
                    supplierName,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                    );
                var invoice = new IncomingInvoice();
                invoice.RaiseEvent(@event);
                return invoice;
            }
        }
    }
}

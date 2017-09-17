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
        IApplyEvent<IncomingInvoiceRegisteredEvent>,
        IApplyEvent<IncomingInvoiceExpiredEvent>
    {
        public PartyInfo Supplier { get; protected set; }

        protected IncomingInvoice()
        {

        }

        public void ApplyEvent([AggregateId(nameof(IncomingInvoiceRegisteredEvent.InvoiceId))] IncomingInvoiceRegisteredEvent evt)
        {
            Id = evt.InvoiceId;
            IsExpired = false;
            Number = evt.InvoiceNumber;
            Date = evt.InvoiceDate;
            DueDate = evt.DueDate;
            Amount = evt.TaxableAmount;
            Taxes = evt.Taxes;
            TotalPrice = evt.TotalPrice;
            Description = evt.Description;
            PaymentTerms = evt.PaymentTerms;
            PurchaseOrderNumber = evt.PurchaseOrderNumber;
            Supplier = new PartyInfo(evt.Supplier.Id, evt.Supplier.Name, evt.Supplier.StreetName, evt.Supplier.City, evt.Supplier.PostalCode, evt.Supplier.Country, evt.Supplier.VatIndex, evt.Supplier.NationalIdentificationNumber);
        }

        public void ApplyEvent([AggregateId(nameof(IncomingInvoicePaidEvent.InvoiceId))] IncomingInvoicePaidEvent evt)
        {
            PaymentDate = evt.PaymentDate;
        }

        public void ApplyEvent([AggregateId(nameof(IncomingInvoiceExpiredEvent.InvoiceId))] IncomingInvoiceExpiredEvent evt)
        {
            IsExpired = true;
        }

        public void MarkAsExpired()
        {
            if (!DueDate.HasValue)
                throw new InvalidOperationException("An invoice must have a due date for it to be marked as expired.");

            var evt = new IncomingInvoiceExpiredEvent(this.Id, DueDate.Value);
            RaiseEvent(evt);
        }

        public void MarkAsPaid(DateTime paymentDate)
        {
            var evt = new IncomingInvoicePaidEvent(this.Id, paymentDate);
            RaiseEvent(evt);
        }

        public static class Factory
        {
            public static IncomingInvoice Register(string invoiceNumber, DateTime invoiceDate, DateTime? dueDate, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, 
                Guid supplierId, string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationCode)
            {
                var @event = new IncomingInvoiceRegisteredEvent(
                        Guid.NewGuid(),
                        invoiceNumber,
                        invoiceDate,
                        dueDate,
                        amount,
                        taxes,
                        totalPrice,
                        description,
                        paymentTerms,
                        purchaseOrderNumber,
                        supplierId,
                        supplierName,
                        supplierAddress,
                        supplierCity,
                        supplierPostalCode,
                        supplierCountry,
                        supplierVatIndex,
                        supplierNationalIdentificationCode
                    );
                var invoice = new IncomingInvoice();
                invoice.RaiseEvent(@event);
                return invoice;
            }

            public static IncomingInvoice Import(Guid invoiceId, string invoiceNumber, DateTime invoiceDate, DateTime? dueDate, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, 
                Guid supplierId, string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationCode)
            {
                var @event = new IncomingInvoiceRegisteredEvent(
                        invoiceId,
                        invoiceNumber,
                        invoiceDate,
                        dueDate,
                        amount,
                        taxes,
                        totalPrice,
                        description,
                        paymentTerms,
                        purchaseOrderNumber,
                        supplierId,
                        supplierName,
                        supplierAddress,
                        supplierCity,
                        supplierPostalCode,
                        supplierCountry,
                        supplierVatIndex,
                        supplierNationalIdentificationCode
                    );
                var invoice = new IncomingInvoice();
                invoice.RaiseEvent(@event);
                return invoice;
            }
        }
    }
}

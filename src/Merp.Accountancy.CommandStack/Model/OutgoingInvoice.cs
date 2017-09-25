using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;
using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.CommandStack.Services;

namespace Merp.Accountancy.CommandStack.Model
{
    public class OutgoingInvoice : Invoice,
        IApplyEvent<OutgoingInvoiceIssuedEvent>,
        IApplyEvent<OutgoingInvoiceGotOverdueEvent>
    {
        public PartyInfo Customer { get; protected set; }

        protected OutgoingInvoice()
        {

        }

        public void ApplyEvent([AggregateId(nameof(OutgoingInvoiceIssuedEvent.InvoiceId))] OutgoingInvoiceIssuedEvent evt)
        {
            Id = evt.InvoiceId;
            IsOverdue = false;
            Number = evt.InvoiceNumber;
            Date = evt.InvoiceDate;
            DueDate = evt.DueDate;
            Amount = evt.TaxableAmount;
            Taxes = evt.Taxes;
            TotalPrice = evt.TotalPrice;
            Description = evt.Description;
            PaymentTerms = evt.PaymentTerms;
            PurchaseOrderNumber = evt.PurchaseOrderNumber;
            Customer = new PartyInfo(evt.Customer.Id, evt.Customer.Name, evt.Customer.StreetName, evt.Customer.City, evt.Customer.PostalCode, evt.Customer.Country, evt.Customer.VatIndex, evt.Customer.NationalIdentificationNumber);
        }

        public void ApplyEvent([AggregateId(nameof(OutgoingInvoicePaidEvent.InvoiceId))] OutgoingInvoicePaidEvent evt)
        {
            PaymentDate = evt.PaymentDate;
        }

        public void ApplyEvent([AggregateId(nameof(OutgoingInvoiceGotOverdueEvent.InvoiceId))] OutgoingInvoiceGotOverdueEvent evt)
        {
            IsOverdue = true;
        }

        public void MarkAsPaid(DateTime paymentDate)
        {
            var evt = new OutgoingInvoicePaidEvent(this.Id, paymentDate);
            RaiseEvent(evt);
        }

        public void MarkAsOverdue()
        {
            if (!DueDate.HasValue)
                throw new InvalidOperationException("An invoice must have a due date for it to be marked as expired.");

            var evt = new OutgoingInvoiceGotOverdueEvent(this.Id, DueDate.Value);
            RaiseEvent(evt);
        }

        public static class Factory
        {
            public static OutgoingInvoice Issue(IOutgoingInvoiceNumberGenerator generator, DateTime invoiceDate, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, Guid customerId, 
                string customerName, string customerAddress, string customerCity, string customerPostalCode, string customerCountry, string customerVatIndex, string customerNationalIdentificationNumber,
                string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationNumber)
            {
                var @event = new OutgoingInvoiceIssuedEvent(
                    Guid.NewGuid(),
                    generator.Generate(),
                    invoiceDate,
                    invoiceDate.AddMonths(1),
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
                    supplierName,
                    supplierAddress,
                    supplierCity,
                    supplierPostalCode,
                    supplierCountry,
                    supplierVatIndex,
                    supplierNationalIdentificationNumber
                    );
                var invoice = new OutgoingInvoice();
                invoice.RaiseEvent(@event);
                return invoice;
            }

            public static OutgoingInvoice Import(Guid invoiceId, string invoiceNumber, DateTime invoiceDate, DateTime? dueDate, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, Guid customerId, 
                string customerName, string customerAddress, string customerCity, string customerPostalCode, string customerCountry, string customerVatIndex, string customerNationalIdentificationNumber,
                string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationNumber)
            {
                var @event = new OutgoingInvoiceIssuedEvent(
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
                    customerId,
                    customerName,
                    customerAddress,
                    customerCity,
                    customerPostalCode,
                    customerCountry,
                    customerVatIndex,
                    customerNationalIdentificationNumber,
                    supplierName,
                    supplierAddress,
                    supplierCity,
                    supplierPostalCode,
                    supplierCountry,
                    supplierVatIndex,
                    supplierNationalIdentificationNumber
                    );
                var invoice = new OutgoingInvoice();
                invoice.RaiseEvent(@event);
                return invoice;
            }
        }
    }
}

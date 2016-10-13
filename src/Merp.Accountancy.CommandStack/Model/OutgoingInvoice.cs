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
        IApplyEvent<OutgoingInvoiceIssuedEvent>
    {
        public PartyInfo Customer { get; protected set; }

        protected OutgoingInvoice()
        {

        }

        public void ApplyEvent([AggregateId(nameof(OutgoingInvoiceIssuedEvent.InvoiceId))] OutgoingInvoiceIssuedEvent evt)
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
            Customer = new PartyInfo(evt.Customer.Id, evt.Customer.Name, evt.Customer.StreetName, evt.Customer.City, evt.Customer.PostalCode, evt.Customer.Country, evt.Customer.VatIndex, evt.Customer.NationalIdentificationNumber);
        }

        public static class Factory
        {
            public static OutgoingInvoice Create(IOutgoingInvoiceNumberGenerator generator, DateTime invoiceDate, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, Guid customerId, string customerName)
            {
                var @event = new OutgoingInvoiceIssuedEvent(
                    Guid.NewGuid(),
                    generator.Generate(),
                    invoiceDate,
                    amount,
                    taxes,
                    totalPrice,
                    description,
                    paymentTerms,
                    purchaseOrderNumber,
                    customerId,
                    customerName,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                    );
                var invoice = new OutgoingInvoice();
                invoice.RaiseEvent(@event);
                return invoice;
            }
        }
    }
}

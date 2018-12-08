using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using MementoFX.Domain;

namespace Merp.Accountancy.CommandStack.Model
{
    public abstract class Invoice : Aggregate
    {
        public string Number { get; protected set; }
        public DateTime Date { get; protected set; }
        public decimal Amount { get; protected set; }
        public decimal Taxes { get; protected set; }
        public decimal TotalPrice { get; protected set; }
        public string Description { get; protected set; }
        public string PaymentTerms { get; protected set; }
        public string PurchaseOrderNumber { get; protected set; }
        public DateTime? DueDate { get; protected set; }
        public DateTime? PaymentDate { get; protected set; }
        public string Currency { get; protected set; }
        public bool IsOverdue { get; protected set; }
        public IEnumerable<InvoiceLineItem> InvoiceLineItems { get; protected set; }
        public bool PricesAreVatIncluded { get; protected set; }
        public IEnumerable<InvoicePriceByVat> InvoicePricesByVat { get; protected set; }
        public IEnumerable<NonTaxableItem> NonTaxableItems { get; protected set; }

        public class PartyInfo
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public string StreetName { get; private set; }
            public string City { get; private set; }
            public string PostalCode { get; private set; }
            public string Country { get; private set; }
            public string VatIndex { get; private set; }
            public string NationalIdentificationNumber { get; private set; }

            public PartyInfo(Guid customerId, string customerName, string streetName, string city, string postalCode, string country, string vatIndex, string nationalIdentificationNumber)
            {
                City = city;
                Name=customerName;
                Country = country;
                Id = customerId;
                NationalIdentificationNumber = nationalIdentificationNumber;
                PostalCode = postalCode;
                StreetName=streetName;
                VatIndex = vatIndex;
            }
        }

        public class InvoiceLineItem
        {
            public string Code { get; set; }

            public string Description { get; set; }

            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal TotalPrice { get; set; }

            public decimal Vat { get; set; }

            public InvoiceLineItem(string code, string description, int quantity, decimal unitPrice, decimal totalPrice, decimal vat)
            {
                Code = code;
                Description = description;
                Quantity = quantity;
                UnitPrice = unitPrice;
                TotalPrice = totalPrice;
                Vat = vat;
            }
        }

        public class InvoicePriceByVat
        {
            public decimal TaxableAmount { get; set; }

            public decimal VatRate { get; set; }

            public decimal VatAmount { get; set; }

            public decimal TotalPrice { get; set; }

            public InvoicePriceByVat(decimal taxableAmount, decimal vatRate, decimal vatAmount, decimal totalPrice)
            {
                TaxableAmount = taxableAmount;
                VatRate = vatRate;
                VatAmount = vatAmount;
                TotalPrice = totalPrice;
            }
        }

        public class NonTaxableItem
        {
            public string Description { get; set; }

            public decimal Amount { get; set; }

            public NonTaxableItem(string description, decimal amount)
            {
                Description = description;
                Amount = amount;
            }
        }
    }
}

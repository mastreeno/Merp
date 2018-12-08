using MementoFX.Domain;
using Merp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingCreditNoteRegisteredEvent : MerpDomainEvent
    {
        public class PartyInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string StreetName { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string VatIndex { get; set; }
            public string NationalIdentificationNumber { get; set; }

            public PartyInfo(Guid partyId, string partyName, string address, string city, string postalCode, string country, string vatIndex, string nationalIdentificationNumber)
            {
                City = city;
                Name = partyName;
                Country = country;
                Id = partyId;
                NationalIdentificationNumber = nationalIdentificationNumber;
                PostalCode = postalCode;
                StreetName = address;
                VatIndex = vatIndex;
            }
        }

        public class LineItem
        {
            public string Code { get; set; }

            public string Description { get; set; }

            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal TotalPrice { get; set; }

            public decimal Vat { get; set; }

            public LineItem(string code, string description, int quantity, decimal unitPrice, decimal totalPrice, decimal vat)
            {
                Code = code;
                Description = description;
                Quantity = quantity;
                UnitPrice = unitPrice;
                TotalPrice = totalPrice;
                Vat = vat;
            }
        }

        public class PriceByVat
        {
            public decimal TaxableAmount { get; set; }

            public decimal VatRate { get; set; }

            public decimal VatAmount { get; set; }

            public decimal TotalPrice { get; set; }

            public PriceByVat(decimal taxableAmount, decimal vatRate, decimal vatAmount, decimal totalPrice)
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

        public Guid CreditNoteId { get; set; }
        public string CreditNoteNumber { get; set; }
        public PartyInfo Customer { get; set; }
        public PartyInfo Supplier { get; set; }
        [Timestamp]
        public DateTime CreditNoteDate { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public string PaymentTerms { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Currency { get; set; }
        public IEnumerable<LineItem> LineItems { get; set; }
        public bool PricesAreVatIncluded { get; set; }
        public IEnumerable<PriceByVat> PricesByVat { get; set; }
        public IEnumerable<NonTaxableItem> NonTaxableItems { get; set; }

        public IncomingCreditNoteRegisteredEvent(Guid creditNoteId, string creditNoteNumber, DateTime creditNoteDate, string currency, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber,
            Guid customerId, string customerName, string customerAddress, string customerCity, string customerPostalCode, string customerCountry, string customerVatIndex, string customerNationalIdentificationNumber,
            Guid supplierId, string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationNumber, IEnumerable<LineItem> lineItems, bool pricesAreVatIncluded, IEnumerable<PriceByVat> pricesByVat, IEnumerable<NonTaxableItem> nonTaxableItems, Guid userId)
            : base(userId)
        {
            var customer = new PartyInfo(
                city: customerCity,
                partyName: customerName,
                country: customerCountry,
                partyId: customerId,
                nationalIdentificationNumber: customerNationalIdentificationNumber,
                postalCode: customerPostalCode,
                address: customerAddress,
                vatIndex: customerVatIndex
            );
            var supplier = new PartyInfo(
                partyId: supplierId,
                city: supplierCity,
                partyName: supplierName,
                country: supplierCountry,
                nationalIdentificationNumber: supplierNationalIdentificationNumber,
                postalCode: supplierPostalCode,
                address: supplierAddress,
                vatIndex: supplierVatIndex
            );
            Customer = customer;
            Supplier = supplier;
            CreditNoteId = creditNoteId;
            CreditNoteNumber = creditNoteNumber;
            CreditNoteDate = creditNoteDate;
            Currency = currency;
            TaxableAmount = amount;
            Taxes = taxes;
            TotalPrice = totalPrice;
            Description = description;
            PaymentTerms = paymentTerms;
            PurchaseOrderNumber = purchaseOrderNumber;
            LineItems = lineItems;
            PricesAreVatIncluded = pricesAreVatIncluded;
            PricesByVat = pricesByVat;
            NonTaxableItems = nonTaxableItems;
        }
    }
}

using MementoFX.Domain;
using Merp.Domain;
using System;
using System.Collections.Generic;

namespace Merp.Accountancy.CommandStack.Events
{
    public class OutgoingCreditNoteIssuedEvent : MerpDomainEvent
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

        public Guid CreditNoteId { get; private set; }

        public PartyInfo Customer { get; set; }

        public PartyInfo Supplier { get; set; }

        public string CreditNoteNumber { get; private set; }

        [Timestamp]
        public DateTime CreditNoteDate { get; private set; }

        public string Currency { get; private set; }

        public decimal TaxableAmount { get; private set; }

        public decimal Taxes { get; private set; }
        
        public decimal TotalPrice { get; private set; }

        public string Description { get; private set; }

        public string PaymentTerms { get; private set; }

        public string PurchaseOrderNumber { get; private set; }

        public IEnumerable<LineItem> LineItems { get; private set; }

        public bool PricesAreVatIncluded { get; private set; }

        public IEnumerable<PriceByVat> PricesByVat { get; private set; }

        public IEnumerable<NonTaxableItem> NonTaxableItems { get; private set; }

        public OutgoingCreditNoteIssuedEvent(Guid creditNoteId, string creditNoteNumber, DateTime creditNoteDate, string currency, decimal taxableAmount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber,
            Guid customerId, string customerName, string customerAddress, string customerCity, string customerPostalCode, string customerCountry, string customerVatIndex, string customerNationalIdentificationNumber,
            string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationNumber, IEnumerable<LineItem> lineItems, bool pricesAreVatIncluded, IEnumerable<PriceByVat> pricesByVat, IEnumerable<NonTaxableItem> nonTaxableItems, Guid userId)
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
                partyId: Guid.Empty,
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
            TaxableAmount = taxableAmount;
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

using System;
using System.Collections.Generic;
using Merp.Domain;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class IssueInvoiceCommand : MerpCommand
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

        public class InvoiceLineItem
        {
            public string Code { get; set; }

            public string Description { get; set; }

            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal TotalPrice { get; set; }

            public decimal Vat { get; set; }

            public string VatDescription { get; set; }

            public InvoiceLineItem(string code, string description, int quantity, decimal unitPrice, decimal totalPrice, decimal vat, string vatDescription)
            {
                Code = code;
                Description = description;
                Quantity = quantity;
                UnitPrice = unitPrice;
                TotalPrice = totalPrice;
                Vat = vat;
                VatDescription = vatDescription;
            }
        }

        public class InvoicePriceByVat
        {
            public decimal TaxableAmount { get; set; }

            public decimal VatRate { get; set; }

            public decimal VatAmount { get; set; }

            public decimal TotalPrice { get; set; }

            public decimal? ProvidenceFundAmount { get; set; }

            public InvoicePriceByVat(decimal taxableAmount, decimal vatRate, decimal vatAmount, decimal totalPrice, decimal? providenceFundAmount)
            {
                TaxableAmount = taxableAmount;
                VatRate = vatRate;
                VatAmount = vatAmount;
                TotalPrice = totalPrice;
                ProvidenceFundAmount = providenceFundAmount;
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

        public Guid InvoiceId { get; set; }
        public PartyInfo Customer { get; set; }
        public PartyInfo Supplier { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Currency { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalToPay { get; set; }
        public string Description { get; set; }
        public string PaymentTerms { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public bool PricesAreVatIncluded { get; set; }
        public IEnumerable<InvoiceLineItem> LineItems { get; set; }
        public IEnumerable<InvoicePriceByVat> PricesByVat { get; set; }
        public IEnumerable<NonTaxableItem> NonTaxableItems { get; set; }
        public string ProvidenceFundDescription { get; set; }
        public decimal? ProvidenceFundRate { get; set; }
        public decimal? ProvidenceFundAmount { get; set; }
        public string WithholdingTaxDescription { get; set; }
        public decimal? WithholdingTaxRate { get; set; }
        public decimal? WithholdingTaxTaxableAmountRate { get; set; }
        public decimal? WithholdingTaxAmount { get; set; }

        public IssueInvoiceCommand(Guid userId, DateTime invoiceDate, string currency, decimal taxableAmount, decimal taxes, decimal totalPrice, decimal totalToPay, string description, string paymentTerms, string purchaseOrderNumber, 
            Guid customerId, string customerName, string customerAddress, string customerCity, string customerPostalCode, string customerCountry, string customerVatIndex, string customerNationalIdentificationNumber,
            string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationNumber, IEnumerable<InvoiceLineItem> lineItems, bool pricesAreVatIncluded, IEnumerable<InvoicePriceByVat> pricesByVat, IEnumerable<NonTaxableItem> nonTaxableItems,
            string providenceFundDescription, decimal? providenceFundRate, decimal? providenceFundAmount, string withholdingTaxDescription, decimal? withholdingTaxRate, decimal? withholdingTaxTaxableAmountRate, decimal? withholdingTaxAmount)
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
            InvoiceDate = invoiceDate;
            Currency = currency;
            TaxableAmount = taxableAmount;
            Taxes = taxes;
            TotalPrice = totalPrice;
            TotalToPay = totalToPay;
            Description = description;
            PaymentTerms = paymentTerms;
            PurchaseOrderNumber = purchaseOrderNumber;
            LineItems = lineItems;
            PricesAreVatIncluded = pricesAreVatIncluded;
            PricesByVat = pricesByVat;
            NonTaxableItems = nonTaxableItems;
            ProvidenceFundDescription = providenceFundDescription;
            ProvidenceFundRate = providenceFundRate;
            ProvidenceFundAmount = providenceFundAmount;
            WithholdingTaxDescription = withholdingTaxDescription;
            WithholdingTaxRate = withholdingTaxRate;
            WithholdingTaxTaxableAmountRate = withholdingTaxTaxableAmountRate;
            WithholdingTaxAmount = withholdingTaxAmount;
        }
    }
}

using Merp.Accountancy.CommandStack.Services;
using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class IssueInvoiceCommand : Command
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

        public Guid InvoiceId { get; set; }
        public PartyInfo Customer { get; set; }
        public PartyInfo Supplier { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Currency { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public string PaymentTerms { get; set; }
        public string PurchaseOrderNumber { get; set; }

        public IssueInvoiceCommand(DateTime invoiceDate, string currency, decimal taxableAmount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, 
            Guid customerId, string customerName, string customerAddress, string customerCity, string customerPostalCode, string customerCountry, string customerVatIndex, string customerNationalIdentificationNumber,
            string supplierName, string supplierAddress, string supplierCity, string supplierPostalCode, string supplierCountry, string supplierVatIndex, string supplierNationalIdentificationNumber)
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
            Description = description;
            PaymentTerms = paymentTerms;
            PurchaseOrderNumber = purchaseOrderNumber;
        }
    }
}

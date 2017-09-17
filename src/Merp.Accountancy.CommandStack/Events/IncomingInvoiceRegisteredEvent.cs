using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;
using Merp.Accountancy.CommandStack.Services;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingInvoiceRegisteredEvent : DomainEvent
    {
        public class SupplierInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string StreetName { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string VatIndex { get; set; }
            public string NationalIdentificationNumber { get; set; }

            public SupplierInfo(Guid supplierId, string supplierName, string streetName, string city, string postalCode, string country, string vatIndex, string nationalIdentificationNumber)
            {
                City = city;
                Name=supplierName;
                Country = country;
                Id = supplierId;
                NationalIdentificationNumber = nationalIdentificationNumber;
                PostalCode = postalCode;
                StreetName=streetName;
                VatIndex = vatIndex;
            }
        }
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public SupplierInfo Supplier { get; set; }
        [Timestamp]
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public string PaymentTerms { get; set; }
        public string PurchaseOrderNumber { get; set; }

        public IncomingInvoiceRegisteredEvent(Guid invoiceId, string invoiceNumber, DateTime invoiceDate, DateTime? dueDate, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, Guid supplierId, string supplierName, string streetName, string city, string postalCode, string country, string vatIndex, string nationalIdentificationNumber)
        {
            var supplier = new SupplierInfo(
                city: city,
                supplierName: supplierName,
                country: country,
                supplierId: supplierId,
                nationalIdentificationNumber: nationalIdentificationNumber,
                postalCode: postalCode,
                streetName: streetName,
                vatIndex: vatIndex
            );
            Supplier = supplier;
            InvoiceId = invoiceId;
            InvoiceNumber = invoiceNumber;
            InvoiceDate = invoiceDate;
            DueDate = dueDate;
            TaxableAmount = amount;
            Taxes = taxes;
            TotalPrice = totalPrice;
            Description = description;
            PaymentTerms = paymentTerms;
            PurchaseOrderNumber = purchaseOrderNumber;
        }
    }
}

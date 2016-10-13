using Merp.Accountancy.CommandStack.Services;
using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class IssueInvoiceCommand : Command
    {
        public class CustomerInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string StreetName { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string VatIndex { get; set; }
            public string NationalIdentificationNumber { get; set; }

            public CustomerInfo(Guid customerId, string customerName, string streetName, string city, string postalCode, string country, string vatIndex, string nationalIdentificationNumber)
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

        public Guid InvoiceId { get; set; }
        public CustomerInfo Customer { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public string PaymentTerms { get; set; }
        public string PurchaseOrderNumber { get; set; }

        public IssueInvoiceCommand(DateTime invoiceDate, decimal amount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, Guid customerId, string customerName, string streetName, string city, string postalCode, string country, string vatIndex, string nationalIdentificationNumber)
        {
            var customer = new CustomerInfo(
                city: city,
                customerName: customerName,
                country: country,
                customerId: customerId,
                nationalIdentificationNumber: nationalIdentificationNumber,
                postalCode: postalCode,
                streetName: streetName,
                vatIndex: vatIndex
            );
            Customer = customer;
            InvoiceDate = invoiceDate;
            Amount = amount;
            Taxes = taxes;
            TotalPrice = totalPrice;
            Description = description;
            PaymentTerms = paymentTerms;
            PurchaseOrderNumber = purchaseOrderNumber;
        }
    }
}

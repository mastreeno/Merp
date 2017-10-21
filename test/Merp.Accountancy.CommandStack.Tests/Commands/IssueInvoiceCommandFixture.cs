using System;
using NUnit.Framework;
using Merp.Accountancy.CommandStack.Commands;

namespace Merp.Accountancy.CommandStack.Tests.Commands
{
    [TestFixture]
    public class IssueInvoiceCommandFixture
    {
        [Test]
        public void Ctor_should_set_properties_according_to_parameters()
        {
            DateTime invoiceDate = new DateTime(1990, 11, 11);
            string currency = "EUR";
            decimal amount = 101;
            decimal taxes = 42;
            decimal totalPrice = 143;
            string description = "fake";
            string paymentTerms = "none";
            string purchaseOrderNumber = "42";
            Guid customerId = Guid.NewGuid();
            string customerName = "Managed Designs S.r.l.";
            string customerAddress = "Via Torino 51";
            string customerCity = "Milan";
            string customerPostalCode = "20123";
            string customerCountry ="Italy";
            string customerVatIndex = "04358780965";
            string customerNationalIdentificationNumber = "04358780965";
            string supplierName = "Mastreeno ltd";
            string supplierAddress = "8, Leman street";
            string supplierCity = "London";
            string supplierPostalCode = "";
            string supplierCountry = "England - United Kingdom";
            string supplierVatIndex = "XYZ";
            string supplierNationalIdentificationNumber = "04358780965";
            var sut = new IssueInvoiceCommand(
                invoiceDate,
                currency,
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
            Assert.AreEqual(invoiceDate, sut.InvoiceDate);
            Assert.AreEqual(currency, sut.Currency);
            Assert.AreEqual(amount, sut.TaxableAmount);
            Assert.AreEqual(taxes, sut.Taxes);
            Assert.AreEqual(totalPrice, sut.TotalPrice);
            Assert.AreEqual(description, sut.Description);
            Assert.AreEqual(paymentTerms, sut.PaymentTerms);
            Assert.AreEqual(purchaseOrderNumber, sut.PurchaseOrderNumber);
            Assert.AreEqual(customerId, sut.Customer.Id);
            Assert.AreEqual(customerName, sut.Customer.Name);
            Assert.AreEqual(customerAddress, sut.Customer.StreetName);
            Assert.AreEqual(customerCity, sut.Customer.City);
            Assert.AreEqual(customerPostalCode, sut.Customer.PostalCode);
            Assert.AreEqual(customerCountry, sut.Customer.Country);
            Assert.AreEqual(customerVatIndex, sut.Customer.VatIndex);
            Assert.AreEqual(customerNationalIdentificationNumber, sut.Customer.NationalIdentificationNumber);

            Assert.AreEqual(supplierName, sut.Supplier.Name);
            Assert.AreEqual(supplierAddress, sut.Supplier.StreetName);
            Assert.AreEqual(supplierCity, sut.Supplier.City);
            Assert.AreEqual(supplierPostalCode, sut.Supplier.PostalCode);
            Assert.AreEqual(supplierCountry, sut.Supplier.Country);
            Assert.AreEqual(supplierVatIndex, sut.Supplier.VatIndex);
            Assert.AreEqual(supplierNationalIdentificationNumber, sut.Supplier.NationalIdentificationNumber);
        }
    }
}

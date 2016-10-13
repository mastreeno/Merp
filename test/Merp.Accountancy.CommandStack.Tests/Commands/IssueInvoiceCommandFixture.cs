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
            decimal amount = 101;
            decimal taxes = 42;
            decimal totalPrice = 143;
            string description = "fake";
            string paymentTerms = "none";
            string purchaseOrderNumber = "42";
            Guid customerId = Guid.NewGuid();
            string customerName = "Managed Designs S.r.l.";
            string streetName = "Via Torino 51";
            string city = "Milan";
            string postalCode = "20123";
            string country ="Italy";
            string vatIndex = "04358780965";
            string nationalIdentificationNumber = "04358780965";
            var sut = new IssueInvoiceCommand(
                invoiceDate,
                amount,
                taxes,
                totalPrice,
                description,
                paymentTerms,
                purchaseOrderNumber,
                customerId,
                customerName,
                streetName,
                city,
                postalCode,
                country,
                vatIndex,
                nationalIdentificationNumber);
            Assert.AreEqual(invoiceDate, sut.InvoiceDate);
            Assert.AreEqual(amount, sut.Amount);
            Assert.AreEqual(taxes, sut.Taxes);
            Assert.AreEqual(totalPrice, sut.TotalPrice);
            Assert.AreEqual(description, sut.Description);
            Assert.AreEqual(paymentTerms, sut.PaymentTerms);
            Assert.AreEqual(purchaseOrderNumber, sut.PurchaseOrderNumber);
            Assert.AreEqual(customerId, sut.Customer.Id);
            Assert.AreEqual(customerName, sut.Customer.Name);
            Assert.AreEqual(streetName, sut.Customer.StreetName);
            Assert.AreEqual(city, sut.Customer.City);
            Assert.AreEqual(postalCode, sut.Customer.PostalCode);
            Assert.AreEqual(country, sut.Customer.Country);
            Assert.AreEqual(vatIndex, sut.Customer.VatIndex);
            Assert.AreEqual(nationalIdentificationNumber, sut.Customer.NationalIdentificationNumber);
        }
    }
}

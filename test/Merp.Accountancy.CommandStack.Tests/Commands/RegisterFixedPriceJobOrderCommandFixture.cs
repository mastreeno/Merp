using System;
using NUnit.Framework;
using Merp.Accountancy.CommandStack.Commands;

namespace Merp.Accountancy.CommandStack.Tests.Commands
{
    [TestFixture]
    public class RegisterFixedPriceJobOrderCommandFixture
    {
        [Test]
        public void Ctor_should_set_properties_according_to_parameters()
        {
            DateTime dateOfStart = new DateTime(1990, 11, 11);
            DateTime dueDate = new DateTime(1990, 11, 12);
            decimal price = 143;
            string currency = "EUR";
            string jobOrderName = "fake";
            Guid customerId = Guid.NewGuid();
            Guid managerId = Guid.NewGuid();
            string purchaseOrderNumber = "42";
            string description = "xyz";
            var sut = new RegisterFixedPriceJobOrderCommand(
                customerId,
                managerId,
                price,
                currency,
                dateOfStart,
                dueDate,
                jobOrderName,
                purchaseOrderNumber,
                description
                );
            Assert.AreEqual(dateOfStart, sut.DateOfStart);
            Assert.AreEqual(dueDate, sut.DueDate);
            Assert.AreEqual(price, sut.Price);
            Assert.AreEqual(currency, sut.Currency);
            Assert.AreEqual(customerId, sut.CustomerId);
            Assert.AreEqual(jobOrderName, sut.JobOrderName);
            Assert.AreEqual(customerId, sut.CustomerId);
            Assert.AreEqual(managerId, sut.ManagerId);
            Assert.AreEqual(purchaseOrderNumber, sut.PurchaseOrderNumber);
            Assert.AreEqual(description, sut.Description);
        }
    }
}

using System;
using NUnit.Framework;
using Merp.Accountancy.CommandStack.Commands;

namespace Merp.Accountancy.CommandStack.Tests.Commands
{
    [TestFixture]
    public class RegisterJobOrderCommandFixture
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
            bool isTimeAndMaterial = true;
            var sut = new RegisterJobOrderCommand(
                customerId,
                managerId,
                price,
                currency,
                dateOfStart,
                dueDate,
                isTimeAndMaterial,
                jobOrderName,
                purchaseOrderNumber,
                description
                );
            Assert.AreEqual(dateOfStart, sut.DateOfStart);
            Assert.AreEqual(dueDate, sut.DueDate);
            Assert.AreEqual(isTimeAndMaterial, sut.IsTimeAndMaterial);
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

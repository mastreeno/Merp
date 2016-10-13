using System;
using NUnit.Framework;
using Merp.Accountancy.CommandStack.Commands;

namespace Merp.Accountancy.CommandStack.Tests.Commands
{
    [TestFixture]
    public class RegisterTimeAndMaterialJobOrderCommandFixture
    {
        [Test]
        public void Ctor_should_set_properties_according_to_parameters()
        {
            DateTime dateOfStart = new DateTime(1990, 11, 11);
            DateTime? dateOfExpiration = new DateTime(1990, 11, 12);
            decimal value = 143;
            string currency = "EUR";
            string jobOrderName = "fake";
            Guid customerId = Guid.NewGuid();
            Guid managerId = Guid.NewGuid();
            string purchaseOrderNumber = "42";
            string description = "xyz";
            var sut = new RegisterTimeAndMaterialJobOrderCommand(
                customerId,
                managerId,
                value,
                currency,
                dateOfStart,
                dateOfExpiration,
                jobOrderName,
                purchaseOrderNumber,
                description
                );
            Assert.AreEqual(dateOfStart, sut.DateOfStart);
            Assert.AreEqual(dateOfExpiration, sut.DateOfExpiration);
            Assert.AreEqual(value, sut.Value);
            Assert.AreEqual(currency, sut.Currency);
            Assert.AreEqual(customerId, sut.CustomerId);
            Assert.AreEqual(jobOrderName, sut.JobOrderName);
            Assert.AreEqual(managerId, sut.ManagerId);
            Assert.AreEqual(purchaseOrderNumber, sut.PurchaseOrderNumber);
            Assert.AreEqual(description, sut.Description);
        }
    }
}

using System;
using Xunit;
using Merp.Accountancy.CommandStack.Commands;

namespace Merp.Accountancy.CommandStack.Tests.Commands
{
    
    public class RegisterJobOrderCommandFixture
    {
        [Fact]
        public void Ctor_should_set_properties_according_to_parameters()
        {
            DateTime dateOfStart = new DateTime(1990, 11, 11);
            DateTime dueDate = new DateTime(1990, 11, 12);
            decimal price = 143;
            string currency = "EUR";
            string jobOrderName = "fake";
            Guid customerId = Guid.NewGuid();
            string customerName = string.Empty;
            Guid managerId = Guid.NewGuid();
            string purchaseOrderNumber = "42";
            string description = "xyz";
            bool isTimeAndMaterial = true;
            var sut = new RegisterJobOrderCommand(
                customerId,
                customerName,
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
            Assert.Equal(dateOfStart, sut.DateOfStart);
            Assert.Equal(dueDate, sut.DueDate);
            Assert.Equal(isTimeAndMaterial, sut.IsTimeAndMaterial);
            Assert.Equal(price, sut.Price);
            Assert.Equal(currency, sut.Currency);
            Assert.Equal(customerId, sut.CustomerId);
            Assert.Equal(jobOrderName, sut.JobOrderName);
            Assert.Equal(customerId, sut.CustomerId);
            Assert.Equal(managerId, sut.ManagerId);
            Assert.Equal(purchaseOrderNumber, sut.PurchaseOrderNumber);
            Assert.Equal(description, sut.Description);
        }
    }
}

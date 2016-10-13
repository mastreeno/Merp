using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;

namespace Merp.Accountancy.CommandStack.Commands
{
    public sealed class RegisterFixedPriceJobOrderCommand : Command
    {
        public Guid JobOrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ManagerId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DueDate { get; set; }
        public string JobOrderName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public RegisterFixedPriceJobOrderCommand(Guid customerId, Guid managerId, decimal price, string currency, DateTime dateOfStart, DateTime dueDate, string jobOrderName, string purchaseOrderNumber, string description)
        {
            CustomerId = customerId;
            ManagerId = managerId;
            Price = price;
            Currency = currency;
            DateOfStart = dateOfStart;
            DueDate = dueDate;
            JobOrderName = jobOrderName;
            PurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }
    }
}

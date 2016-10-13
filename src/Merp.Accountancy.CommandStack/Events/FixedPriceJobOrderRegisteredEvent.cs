using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;

namespace Merp.Accountancy.CommandStack.Events
{
    public class FixedPriceJobOrderRegisteredEvent : DomainEvent
    {
        public Guid JobOrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ManagerId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        [Timestamp]
        public DateTime DateOfStart { get; set; }
        public DateTime DueDate { get; set; }
        public string JobOrderName { get; set; }
        public string JobOrderNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public FixedPriceJobOrderRegisteredEvent(Guid jobOrderId, Guid customerId, Guid managerId, decimal price, string currency, DateTime dateOfStart, DateTime dueDate, string jobOrderName, string jobOrderNumber, string purchaseOrderNumber, string description)
        {
            JobOrderId = jobOrderId;
            CustomerId = customerId;
            ManagerId = managerId;
            Price = price;
            Currency = currency;
            DateOfStart = dateOfStart;
            DueDate = dueDate;
            JobOrderName = jobOrderName;
            JobOrderNumber = jobOrderNumber;
            PurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }
    }
}

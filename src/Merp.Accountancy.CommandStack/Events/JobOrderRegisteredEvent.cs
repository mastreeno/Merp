using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using MementoFX.Domain;

namespace Merp.Accountancy.CommandStack.Events
{
    public class JobOrderRegisteredEvent : DomainEvent
    {
        public Guid JobOrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid ManagerId { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        [Timestamp]
        public DateTime DateOfRegistration { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsTimeAndMaterial { get; set; }
        public string JobOrderName { get; set; }
        public string JobOrderNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public JobOrderRegisteredEvent(Guid jobOrderId, Guid customerId, string customerName, Guid managerId, decimal? price, string currency, DateTime dateOfRegistration, DateTime dateOfStart, DateTime dueDate, bool isTimeAndMaterial, string jobOrderName, string jobOrderNumber, string purchaseOrderNumber, string description)
        {
            JobOrderId = jobOrderId;
            CustomerId = customerId;
            CustomerName = customerName;
            ManagerId = managerId;
            Price = price;
            Currency = currency;
            DateOfRegistration = dateOfRegistration;
            DateOfStart = dateOfStart;
            DueDate = dueDate;
            IsTimeAndMaterial = isTimeAndMaterial;
            JobOrderName = jobOrderName;
            JobOrderNumber = jobOrderNumber;
            PurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using MementoFX.Domain;

namespace Merp.Sales.CommandStack.Events
{
    public class ProjectRegisteredEvent : DomainEvent
    {
        public Guid ProjectId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? ContactPersonId { get; set; }
        public Guid ManagerId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        [Timestamp]
        public DateTime DateOfRegistration { get; set; }
        public DateTime? DateOfStart { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsTimeAndMaterial { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public string CustomerPurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public ProjectRegisteredEvent(Guid jobOrderId, Guid customerId, Guid? contactPersonId, Guid managerId, decimal price, string currency, DateTime dateOfRegistration, DateTime? dateOfStart, DateTime? dueDate, bool isTimeAndMaterial, string projectName, string projectNumber, string purchaseOrderNumber, string description)
        {
            ProjectId = jobOrderId;
            CustomerId = customerId;
            ContactPersonId = contactPersonId;
            ManagerId = managerId;
            Price = price;
            Currency = currency;
            DateOfRegistration = dateOfRegistration;
            DateOfStart = dateOfStart;
            DueDate = dueDate;
            IsTimeAndMaterial = isTimeAndMaterial;
            ProjectName = projectName;
            ProjectNumber = projectNumber;
            CustomerPurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }
    }
}

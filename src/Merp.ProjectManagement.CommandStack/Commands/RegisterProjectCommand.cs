using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;

namespace Merp.ProjectManagement.CommandStack.Commands
{
    public sealed class RegisterProjectCommand : Command
    {
        public Guid ProjectId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? ContactPersonId { get; set; }
        public Guid ManagerId { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsTimeAndMaterial { get; set; }
        public string ProjectName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public RegisterProjectCommand(Guid customerId, string customerName, Guid? contactPersonId, Guid managerId, decimal price, string currency, DateTime dateOfStart, DateTime dueDate, bool isTimeAndMaterial, string projectName, string purchaseOrderNumber, string description)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            ContactPersonId = contactPersonId;
            ManagerId = managerId;
            Price = price;
            Currency = currency;
            DateOfStart = dateOfStart;
            DueDate = dueDate;
            IsTimeAndMaterial = isTimeAndMaterial;
            ProjectName = projectName;
            PurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using Merp.Domain.Model;
using Merp.ProjectManagement.CommandStack.Model;

namespace Merp.ProjectManagement.CommandStack.Commands
{
    public sealed class RegisterProjectCommand : Command
    {
        public Guid ProjectId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? ContactPersonId { get; set; }
        public Guid ManagerId { get; set; }
        public Money Price { get; set; }
        public DateTime? DateOfStart { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsTimeAndMaterial { get; set; }
        public string ProjectName { get; set; }
        public string CustomerPurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public RegisterProjectCommand(Guid customerId, Guid? contactPersonId, Guid managerId, Money price, DateTime? dateOfStart, DateTime? dueDate, bool isTimeAndMaterial, string projectName, string purchaseOrderNumber, string description)
        {
            CustomerId = customerId;
            ContactPersonId = contactPersonId;
            ManagerId = managerId;
            Price = price;
            DateOfStart = dateOfStart;
            DueDate = dueDate;
            IsTimeAndMaterial = isTimeAndMaterial;
            ProjectName = projectName;
            CustomerPurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }
    }
}

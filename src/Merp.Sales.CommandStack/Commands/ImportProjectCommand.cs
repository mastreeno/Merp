using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using Merp.Domain.Model;
using Merp.Sales.CommandStack.Model;

namespace Merp.Sales.CommandStack.Commands
{
    public sealed class ImportProjectCommand : Command
    {
        public Guid ProjectId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? ContactPersonId { get; set; }
        public Guid ManagerId { get; set; }
        public Money Price { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsTimeAndMaterial { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public ImportProjectCommand(Guid projectId, Guid customerId, Guid managerId, Money price, string currency, DateTime dateOfRegistration, DateTime dateOfStart, DateTime dueDate, bool isTimeAndMaterial, string jobOrderNumber, string jobOrderName, string purchaseOrderNumber, string description)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentException("A non empty project id must me specified.", nameof(projectId));

            ProjectId = projectId;
            CustomerId = customerId;
            ManagerId = managerId;
            Price = price;
            DateOfRegistration = dateOfRegistration;
            DateOfStart = dateOfStart;
            DueDate = dueDate;
            IsTimeAndMaterial = isTimeAndMaterial;
            ProjectNumber = jobOrderNumber;
            ProjectName = jobOrderName;
            PurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }
    }
}

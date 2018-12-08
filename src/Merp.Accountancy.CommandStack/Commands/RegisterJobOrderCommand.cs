using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using Merp.Domain;

namespace Merp.Accountancy.CommandStack.Commands
{
    public sealed class RegisterJobOrderCommand : MerpCommand
    {
        public Guid JobOrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? ContactPersonId { get; set; }
        public Guid ManagerId { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsTimeAndMaterial { get; set; }
        public string JobOrderName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public RegisterJobOrderCommand(Guid userId, Guid customerId, string customerName, Guid? contactPersonId, Guid managerId, decimal price, string currency, DateTime dateOfStart, DateTime dueDate, bool isTimeAndMaterial, string jobOrderName, string purchaseOrderNumber, string description)
            : base(userId)
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
            JobOrderName = jobOrderName;
            PurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }
    }
}

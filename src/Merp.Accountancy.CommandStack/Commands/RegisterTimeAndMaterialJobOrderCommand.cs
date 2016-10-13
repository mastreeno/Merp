using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;

namespace Merp.Accountancy.CommandStack.Commands
{
    public sealed class RegisterTimeAndMaterialJobOrderCommand : Command
    {
        public Guid JobOrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ManagerId { get; set; }
        public decimal Value { get; set; }
        public string Currency { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime? DateOfExpiration { get; set; }
        public string JobOrderName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public RegisterTimeAndMaterialJobOrderCommand(Guid customerId, Guid managerId, decimal value, string currency, DateTime dateOfStart, DateTime? dateOfExpiration, string jobOrderName, string purchaseOrderNumber, string description)
        {
            CustomerId = customerId;
            ManagerId = managerId;
            Value = value;
            Currency = currency;
            DateOfStart = dateOfStart;
            DateOfExpiration = dateOfExpiration;
            JobOrderName = jobOrderName;
            PurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }
    }
}

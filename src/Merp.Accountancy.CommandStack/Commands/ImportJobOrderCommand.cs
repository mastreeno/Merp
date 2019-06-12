using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using Merp.Domain;

namespace Merp.Accountancy.CommandStack.Commands
{
    public sealed class ImportJobOrderCommand : MerpCommand
    {
        public Guid JobOrderId { get; set; }
        public PartyInfo Customer { get; set; }
        public Guid? ContactPersonId { get; set; }
        public Guid ManagerId { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsTimeAndMaterial { get; set; }
        public string JobOrderNumber { get; set; }
        public string JobOrderName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Description { get; set; }

        public ImportJobOrderCommand(Guid userId, Guid jobOrderId, Guid customerId, string customerName, Guid managerId, decimal? price, string currency, DateTime dateOfRegistration, DateTime dateOfStart, DateTime dueDate, bool isTimeAndMaterial, string jobOrderNumber, string jobOrderName, string purchaseOrderNumber, string description)
            : base(userId)
        {
            JobOrderId = jobOrderId;
            Customer = new PartyInfo(customerId, customerName);
            ManagerId = managerId;
            Price = price;
            Currency = currency;
            DateOfRegistration = dateOfRegistration;
            DateOfStart = dateOfStart;
            DueDate = dueDate;
            IsTimeAndMaterial = isTimeAndMaterial;
            JobOrderNumber = jobOrderNumber;
            JobOrderName = jobOrderName;
            PurchaseOrderNumber = purchaseOrderNumber;
            Description = description;
        }

        public class PartyInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public PartyInfo(Guid customerId, string customerName)
            {
                Name = customerName;
                Id = customerId;
            }
        }
    }
}

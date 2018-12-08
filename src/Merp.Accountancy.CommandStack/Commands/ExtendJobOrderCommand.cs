using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class ExtendJobOrderCommand : MerpCommand
    {
        public Guid JobOrderId { get; set; }
        public DateTime NewDueDate { get; set; }
        public decimal Price { get; set; }

        public ExtendJobOrderCommand(Guid userId, Guid jobOrderId, DateTime newDueDate, decimal price)
            : base(userId)
        {
            JobOrderId = jobOrderId;
            NewDueDate = newDueDate;
            Price = price;
        }
    }
}

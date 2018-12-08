using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Events
{
    public class JobOrderExtendedEvent : MerpDomainEvent
    {
        public Guid JobOrderId { get; set; }
        public DateTime NewDueDate { get; set; }
        public decimal Price { get; set; }

        public JobOrderExtendedEvent(Guid jobOrderId, DateTime newDueDate, decimal price, Guid userId)
            : base(userId)
        {
            JobOrderId = jobOrderId;
            NewDueDate = newDueDate;
            Price = price;
        }
    }
}

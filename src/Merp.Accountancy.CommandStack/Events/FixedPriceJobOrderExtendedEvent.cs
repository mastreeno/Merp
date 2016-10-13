using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Events
{
    public class FixedPriceJobOrderExtendedEvent : DomainEvent
    {
        public Guid JobOrderId { get; set; }
        public DateTime NewDueDate { get; set; }
        public decimal Price { get; set; }

        public FixedPriceJobOrderExtendedEvent(Guid jobOrderId, DateTime newDueDate, decimal price)
        {
            JobOrderId = jobOrderId;
            NewDueDate = newDueDate;
            Price = price;
        }
    }
}

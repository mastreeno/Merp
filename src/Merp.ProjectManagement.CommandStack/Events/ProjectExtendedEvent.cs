using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.ProjectManagement.CommandStack.Events
{
    public class ProjectExtendedEvent : DomainEvent
    {
        public Guid ProjectId { get; set; }
        public DateTime NewDueDate { get; set; }
        public decimal Price { get; set; }

        public ProjectExtendedEvent(Guid jobOrderId, DateTime newDueDate, decimal price)
        {
            ProjectId = jobOrderId;
            NewDueDate = newDueDate;
            Price = price;
        }
    }
}

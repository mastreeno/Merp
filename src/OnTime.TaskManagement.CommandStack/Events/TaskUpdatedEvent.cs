using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTime.TaskManagement.CommandStack.Events
{
    public class TaskUpdatedEvent : DomainEvent
    {
        public Guid TaskId { get; set; }
        public Guid? JobOrderId { get; set; }
        public string Text { get; set; }
    }
}

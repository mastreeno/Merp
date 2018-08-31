using MementoFX;
using Merp.TimeTracking.TaskManagement.CommandStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.TimeTracking.TaskManagement.CommandStack.Events
{
    public class TaskUpdatedEvent : DomainEvent
    {
        public Guid TaskId { get; set; }
        public Guid? JobOrderId { get; set; }
        public string Name { get; set; }
        public TaskPriority Priority { get; set; }
    }
}

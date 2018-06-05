using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTime.TaskManagement.CommandStack.Events
{
    public class TaskReactivatedEvent : DomainEvent
    {
        public Guid TaskId { get; set; }

    }
}

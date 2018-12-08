using MementoFX;
using MementoFX.Domain;
using Merp.Domain;
using Merp.TimeTracking.TaskManagement.CommandStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.TimeTracking.TaskManagement.CommandStack.Events
{
    public class TaskAddedEvent : MerpDomainEvent
    {
        public Guid TaskId { get; set; }

        [Timestamp]
        public DateTime DateOfCreation { get; set; }

        public string TaskName { get; set; }

        public TaskPriority Priority { get; set; }

        public TaskAddedEvent(Guid userId) : base(userId) { }
    }
}

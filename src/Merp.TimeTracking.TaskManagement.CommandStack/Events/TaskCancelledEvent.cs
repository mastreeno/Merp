using MementoFX;
using MementoFX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.TimeTracking.TaskManagement.CommandStack.Events
{
    public class TaskCancelledEvent : DomainEvent
    {
        public Guid TaskId { get; set; }

        [Timestamp]
        public DateTime DateOfCancellation { get; set; }

        public Guid UserId { get; set; }
    }
}

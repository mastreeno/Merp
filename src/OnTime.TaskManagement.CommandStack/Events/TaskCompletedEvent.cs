using MementoFX;
using MementoFX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTime.TaskManagement.CommandStack.Events
{
    public class TaskCompletedEvent : DomainEvent
    {
        public Guid TaskId { get; set; }

        [Timestamp]
        public DateTime DateOfCompletion { get; set; }

        public Guid UserId { get; set; }
    }
}

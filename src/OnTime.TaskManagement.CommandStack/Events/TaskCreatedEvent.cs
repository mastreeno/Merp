using MementoFX;
using MementoFX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTime.TaskManagement.CommandStack.Events
{
    public class TaskCreatedEvent : DomainEvent
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }

        [Timestamp]
        public DateTime DateOfCreation { get; set; }

        public string TaskName { get; set; }
    }
}

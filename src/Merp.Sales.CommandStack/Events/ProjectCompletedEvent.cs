using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using MementoFX.Domain;

namespace Merp.Sales.CommandStack.Events
{
    public class ProjectCompletedEvent : DomainEvent
    {
        public Guid ProjectId { get; set; }
        [Timestamp]
        public DateTime DateOfCompletion { get; set; }

        public ProjectCompletedEvent(Guid jobOrderId, DateTime dateOfCompletion)
        {
            this.ProjectId = jobOrderId;
            this.DateOfCompletion = dateOfCompletion;
        }
    }
}

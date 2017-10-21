using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX;
using MementoFX.Domain;

namespace Merp.Accountancy.CommandStack.Events
{
    public class JobOrderCompletedEvent : DomainEvent
    {
        public Guid JobOrderId { get; set; }
        [Timestamp]
        public DateTime DateOfCompletion { get; set; }

        public JobOrderCompletedEvent(Guid jobOrderId, DateTime dateOfCompletion)
        {
            this.JobOrderId = jobOrderId;
            this.DateOfCompletion = dateOfCompletion;
        }
    }
}

using System;
using MementoFX.Domain;
using Merp.Domain;

namespace Merp.Accountancy.CommandStack.Events
{
    public class JobOrderCompletedEvent : MerpDomainEvent
    {
        public Guid JobOrderId { get; set; }
        [Timestamp]
        public DateTime DateOfCompletion { get; set; }

        public JobOrderCompletedEvent(Guid jobOrderId, DateTime dateOfCompletion, Guid userId)
            : base(userId)
        {
            this.JobOrderId = jobOrderId;
            this.DateOfCompletion = dateOfCompletion;
        }
    }
}

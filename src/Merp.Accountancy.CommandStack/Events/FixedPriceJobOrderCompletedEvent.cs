using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;

namespace Merp.Accountancy.CommandStack.Events
{
    public class FixedPriceJobOrderCompletedEvent : DomainEvent
    {
        public Guid JobOrderId { get; set; }
        [Timestamp]
        public DateTime DateOfCompletion { get; set; }

        public FixedPriceJobOrderCompletedEvent(Guid jobOrderId, DateTime dateOfCompletion)
        {
            this.JobOrderId = jobOrderId;
            this.DateOfCompletion = dateOfCompletion;
        }
    }
}

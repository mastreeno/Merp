using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Events
{
    public class TimeAndMaterialJobOrderExtendedEvent : DomainEvent
    {
        public Guid JobOrderId { get; set; }
        public DateTime? NewDateOfExpiration { get; set; }
        public decimal Value { get; set; }

        public TimeAndMaterialJobOrderExtendedEvent(Guid jobOrderId, DateTime? newDateOfExpiration, decimal value)
        {
            JobOrderId = jobOrderId;
            NewDateOfExpiration = newDateOfExpiration;
            Value = value;
        }
    }
}

using MementoFX;
using MementoFX.Domain;
using System;

namespace Merp.Registry.CommandStack.Events
{
    public class PartyUnlistedEvent : DomainEvent
    {
        public Guid PartyId { get; set; }

        [Timestamp]
        public DateTime UnlistDate { get; set; }

        public PartyUnlistedEvent(Guid partyId, DateTime unlistDate)
        {
            PartyId = partyId;
            UnlistDate = unlistDate;
        }
    }
}

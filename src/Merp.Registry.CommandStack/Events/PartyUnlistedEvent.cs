using MementoFX.Domain;
using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Events
{
    public class PartyUnlistedEvent : MerpDomainEvent
    {
        public Guid PartyId { get; set; }

        [Timestamp]
        public DateTime UnlistDate { get; set; }

        public PartyUnlistedEvent(Guid partyId, DateTime unlistDate, Guid userId)
            : base(userId)
        {
            PartyId = partyId;
            UnlistDate = unlistDate;
        }
    }
}

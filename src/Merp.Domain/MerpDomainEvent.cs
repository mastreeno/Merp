using MementoFX;
using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.Domain
{
    public class MerpDomainEvent : DomainEvent
    {
        public Guid UserId { get; private set; }

        public MerpDomainEvent(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("The userId cannot be blank", nameof(userId));

            this.UserId = userId;
        }
    }
}

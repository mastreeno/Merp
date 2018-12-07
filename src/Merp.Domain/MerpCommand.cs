using MementoFX;
using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.Domain
{
    public class MerpCommand : Command
    {
        public Guid UserId { get; private set; }

        public MerpCommand(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("The userId cannot be blank", nameof(userId));

            this.UserId = userId;
        }
    }
}

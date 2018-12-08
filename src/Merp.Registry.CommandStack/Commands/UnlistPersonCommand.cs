using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class UnlistPersonCommand : MerpCommand
    {
        public Guid PersonId { get; set; }

        public DateTime UnlistDate { get; set; }

        public UnlistPersonCommand(Guid userId, Guid personId, DateTime unlistDate)
            : base(userId)
        {
            PersonId = personId;
            UnlistDate = unlistDate;
        }
    }
}

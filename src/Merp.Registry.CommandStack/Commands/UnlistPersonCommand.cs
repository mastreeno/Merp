using MementoFX;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class UnlistPersonCommand : Command
    {
        public Guid PersonId { get; set; }

        public DateTime UnlistDate { get; set; }

        public UnlistPersonCommand(Guid personId, DateTime unlistDate)
        {
            PersonId = personId;
            UnlistDate = unlistDate;
        }
    }
}

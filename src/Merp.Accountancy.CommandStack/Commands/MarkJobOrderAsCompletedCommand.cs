using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkJobOrderAsCompletedCommand : MerpCommand
    {
        public Guid JobOrderId { get; set; }
        public DateTime DateOfCompletion { get; set; }

        public MarkJobOrderAsCompletedCommand(Guid userId, Guid jobOrderId, DateTime dateOfCompletion)
            : base(userId)
        {
            this.JobOrderId = jobOrderId;
            this.DateOfCompletion = dateOfCompletion;
        }
    }
}

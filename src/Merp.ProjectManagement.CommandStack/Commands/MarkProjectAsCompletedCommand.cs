using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.ProjectManagement.CommandStack.Commands
{
    public class MarkProjectAsCompletedCommand : Command
    {
        public Guid ProjectId { get; set; }
        public DateTime DateOfCompletion { get; set; }

        public MarkProjectAsCompletedCommand(Guid jobOrderId, DateTime dateOfCompletion)
        {
            this.ProjectId = jobOrderId;
            this.DateOfCompletion = dateOfCompletion;
        }
    }
}

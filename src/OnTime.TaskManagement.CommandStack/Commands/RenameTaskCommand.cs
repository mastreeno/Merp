using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTime.TaskManagement.CommandStack.Commands
{
    public class RenameTaskCommand : Command
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }

        public string ProposedName { get; set; }
    }
}

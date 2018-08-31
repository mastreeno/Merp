using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.TimeTracking.TaskManagement.CommandStack.Commands
{
    public class AddTaskCommand : Command
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
    }
}

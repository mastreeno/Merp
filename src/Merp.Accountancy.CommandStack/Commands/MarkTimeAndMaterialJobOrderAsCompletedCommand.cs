using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class MarkTimeAndMaterialJobOrderAsCompletedCommand : Command
    {
        public Guid JobOrderId { get; set; }
        public DateTime DateOfCompletion { get; set; }

        public MarkTimeAndMaterialJobOrderAsCompletedCommand(Guid jobOrderId, DateTime dateOfCompletion)
        {
            this.JobOrderId = jobOrderId;
            this.DateOfCompletion = dateOfCompletion;
        }
    }
}

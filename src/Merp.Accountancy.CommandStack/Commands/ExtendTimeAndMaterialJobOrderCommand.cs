using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class ExtendTimeAndMaterialJobOrderCommand : Command
    {
        public Guid JobOrderId { get; set; }
        public DateTime? NewDateOfExpiration { get; set; }
        public decimal Value { get; set; }

        public ExtendTimeAndMaterialJobOrderCommand(Guid jobOrderId, DateTime? newDateOfExpiration, decimal value)
        {
            JobOrderId = jobOrderId;
            NewDateOfExpiration = newDateOfExpiration;
            Value = value;
        }
    }
}

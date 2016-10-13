using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class ExtendFixedPriceJobOrderCommand : Command
    {
        public Guid JobOrderId { get; set; }
        public DateTime NewDueDate { get; set; }
        public decimal Price { get; set; }

        public ExtendFixedPriceJobOrderCommand(Guid jobOrderId, DateTime newDueDate, decimal price)
        {
            JobOrderId = jobOrderId;
            NewDueDate = newDueDate;
            Price = price;
        }
    }
}

using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Events
{
    public class CompanyMainContactAssociatedEvent : DomainEvent
    {
        public Guid CompanyId { get; set; }
        public Guid MainContactId { get; set; }

        public CompanyMainContactAssociatedEvent(Guid companyId, Guid mainContactId)
        {
            CompanyId = companyId;
            MainContactId = mainContactId;
        }
    }
}

using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Events
{
    public class CompanyAdministrativeContactAssociatedEvent : DomainEvent
    {
        public Guid CompanyId { get; set; }
        public Guid AdministrativeContactId { get; set; }

        public CompanyAdministrativeContactAssociatedEvent(Guid companyId, Guid administrativeContactId)
        {
            CompanyId = companyId;
            AdministrativeContactId = administrativeContactId;
        }
    }
}

using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Events
{
    public class CompanyAdministrativeContactAssociatedEvent : MerpDomainEvent
    {
        public Guid CompanyId { get; set; }
        public Guid AdministrativeContactId { get; set; }

        public CompanyAdministrativeContactAssociatedEvent(Guid companyId, Guid administrativeContactId, Guid userId)
            : base(userId)
        {
            CompanyId = companyId;
            AdministrativeContactId = administrativeContactId;
        }
    }
}

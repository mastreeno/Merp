using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Events
{
    public class CompanyMainContactAssociatedEvent : MerpDomainEvent
    {
        public Guid CompanyId { get; set; }
        public Guid MainContactId { get; set; }

        public CompanyMainContactAssociatedEvent(Guid companyId, Guid mainContactId, Guid userId)
            : base(userId)
        {
            CompanyId = companyId;
            MainContactId = mainContactId;
        }
    }
}

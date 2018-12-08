using System;
using MementoFX.Domain;
using Merp.Domain;

namespace Merp.Registry.CommandStack.Events
{
    public class CompanyNameChangedEvent : MerpDomainEvent
    {
        public Guid CompanyId { get; set; }
        [Timestamp]
        public DateTime EffectiveDate { get; set; }
        public string CompanyName { get; set; }

        public CompanyNameChangedEvent(Guid companyId, string companyName, DateTime effectiveDate, Guid userId)
            : base(userId)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            EffectiveDate = effectiveDate;
        }
    }
}

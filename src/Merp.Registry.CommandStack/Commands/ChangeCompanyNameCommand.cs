using System;
using Merp.Domain;

namespace Merp.Registry.CommandStack.Commands
{
    public class ChangeCompanyNameCommand : MerpCommand
    {
        public Guid CompanyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string CompanyName { get; set; }

        public ChangeCompanyNameCommand(Guid userId, Guid companyId, string companyName, DateTime effectiveDate)
            : base(userId)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            EffectiveDate = effectiveDate;
        }
    }
}

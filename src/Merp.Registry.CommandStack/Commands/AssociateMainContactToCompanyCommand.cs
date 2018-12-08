using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class AssociateMainContactToCompanyCommand : MerpCommand
    {
        public Guid CompanyId { get; private set; }
        public Guid MainContactId { get; private set; }

        public AssociateMainContactToCompanyCommand(Guid userId, Guid companyId, Guid mainContactId)
            : base(userId)
        {
            CompanyId = companyId;
            MainContactId = mainContactId;
        }
    }
}

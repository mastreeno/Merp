using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class AssociateAdministrativeContactToCompanyCommand : MerpCommand
    {
        public Guid CompanyId { get; private set; }
        public Guid AdministrativeContactId { get; private set; }

        public AssociateAdministrativeContactToCompanyCommand(Guid userId, Guid companyId, Guid administrativeContactId)
            : base(userId)
        {
            CompanyId = companyId;
            AdministrativeContactId = administrativeContactId;
        }
    }
}

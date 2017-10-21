using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Commands
{
    public class AssociateAdministrativeContactToCompanyCommand : Command
    {
        public Guid CompanyId { get; private set; }
        public Guid AdministrativeContactId { get; private set; }

        public AssociateAdministrativeContactToCompanyCommand(Guid companyId, Guid administrativeContactId)
        {
            CompanyId = companyId;
            AdministrativeContactId = administrativeContactId;
        }
    }
}

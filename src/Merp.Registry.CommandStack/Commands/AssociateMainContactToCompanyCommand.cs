using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Commands
{
    public class AssociateMainContactToCompanyCommand : Command
    {
        public Guid CompanyId { get; private set; }
        public Guid MainContactId { get; private set; }

        public AssociateMainContactToCompanyCommand(Guid companyId, Guid mainContactId)
        {
            CompanyId = companyId;
            MainContactId = mainContactId;
        }
    }
}

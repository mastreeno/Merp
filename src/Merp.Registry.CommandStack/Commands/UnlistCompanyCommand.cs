using MementoFX;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class UnlistCompanyCommand : Command
    {
        public Guid CompanyId { get; set; }

        public DateTime UnlistDate { get; set; }

        public UnlistCompanyCommand(Guid companyId, DateTime unlistDate)
        {
            CompanyId = companyId;
            UnlistDate = unlistDate;
        }
    }
}

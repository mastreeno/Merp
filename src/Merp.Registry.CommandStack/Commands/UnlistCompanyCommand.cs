using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class UnlistCompanyCommand : MerpCommand
    {
        public Guid CompanyId { get; set; }

        public DateTime UnlistDate { get; set; }

        public UnlistCompanyCommand(Guid userId, Guid companyId, DateTime unlistDate)
            : base(userId)
        {
            CompanyId = companyId;
            UnlistDate = unlistDate;
        }
    }
}

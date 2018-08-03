using Merp.Sales.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Sales.QueryStack
{
    public static class BusinessProposalExtensions
    {
        public static IQueryable<BusinessProposal> CurrentOnly(this IQueryable<BusinessProposal> projects)
        {
            return projects.Where(p => p.IsCompleted == false && p.DateOfStart != null);
        }

        public static IQueryable<BusinessProposal> CompletedOnly(this IQueryable<BusinessProposal> projects)
        {
            return projects.Where(p => p.IsCompleted == true);
        }
    }
}

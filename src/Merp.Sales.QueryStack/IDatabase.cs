using Merp.Sales.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Sales.QueryStack
{
    public interface IDatabase
    {
        IQueryable<BusinessProposal> Proposals { get; }
    }
}

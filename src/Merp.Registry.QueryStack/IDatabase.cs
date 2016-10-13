using Merp.Registry.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.QueryStack
{
    public interface IDatabase
    {
        IQueryable<Party> Parties { get; }
    }
}

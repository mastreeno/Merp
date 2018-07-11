using Merp.ProjectManagement.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.ProjectManagement.QueryStack
{
    public interface IDatabase
    {
        IQueryable<Project> JobOrders { get; }
    }
}

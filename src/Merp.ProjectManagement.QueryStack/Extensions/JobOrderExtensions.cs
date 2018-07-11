using Merp.ProjectManagement.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.ProjectManagement.QueryStack
{
    public static class JobOrderExtensions
    {
        public static IQueryable<Project> CurrentOnly(this IQueryable<Project> jobOrders)
        {
            return jobOrders.Where(jo => jo.IsCompleted == false);
        }
    }
}

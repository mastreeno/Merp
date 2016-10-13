using Merp.Accountancy.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack
{
    public static class JobOrderExtensions
    {
        public static IQueryable<JobOrder> CurrentOnly(this IQueryable<JobOrder> jobOrders)
        {
            return jobOrders.Where(jo => jo.IsCompleted == false);
        }
    }
}

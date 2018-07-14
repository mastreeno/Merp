using Merp.ProjectManagement.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.ProjectManagement.QueryStack
{
    public static class ProjectExtensions
    {
        public static IQueryable<Project> CurrentOnly(this IQueryable<Project> projects)
        {
            return projects.Where(p => p.IsCompleted == false && p.DateOfStart != null);
        }

        public static IQueryable<Project> CompletedOnly(this IQueryable<Project> projects)
        {
            return projects.Where(p => p.IsCompleted == true);
        }
    }
}

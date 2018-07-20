using Merp.Sales.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Sales.QueryStack
{
    public class Database : IDatabase, IDisposable
    {
        private ProjectManagementDbContext Context;

        public Database(ProjectManagementDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IQueryable<Project> Projects
        {
            get
            {
                return Context.Projects;
            }
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}

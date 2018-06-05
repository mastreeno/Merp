using OnTime.TaskManagement.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnTime.TaskManagement.QueryStack
{
    public class Database : IDatabase
    {
        private TaskManagementDbContext Context = null;

        public Database(TaskManagementDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IQueryable<Task> Tasks
        {
            get
            {
                return Context.Tasks;
            }
        }
    }
}

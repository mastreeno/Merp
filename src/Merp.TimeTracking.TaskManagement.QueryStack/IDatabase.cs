using Merp.TimeTracking.TaskManagement.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Merp.TimeTracking.TaskManagement.QueryStack
{
    public interface IDatabase
    {
        IQueryable<Task> Tasks { get; }
    }
}

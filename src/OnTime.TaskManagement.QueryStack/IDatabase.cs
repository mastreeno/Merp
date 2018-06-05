using OnTime.TaskManagement.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnTime.TaskManagement.QueryStack
{
    public interface IDatabase
    {
        IQueryable<Task> Tasks { get; }
    }
}

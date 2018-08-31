using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Merp.TimeTracking.TaskManagement.QueryStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.TimeTracking.TaskManagement.QueryStack
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TaskManagementDbContext>
    {
        public TaskManagementDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TaskManagementDbContext>();
            builder.UseSqlServer("Server=.\\SQLExpress;Database=Merp-TimeTracking-ReadModel;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new TaskManagementDbContext(builder.Options);
        }
    }
}

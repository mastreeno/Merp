using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.ProjectManagement.QueryStack
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ProjectManagementDbContext>
    {
        public ProjectManagementDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ProjectManagementDbContext>();
            builder.UseSqlServer("Server=.\\SQLExpress;Database=Merp-ProjectManagement-ReadModel;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new ProjectManagementDbContext(builder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.Sales.QueryStack
{
    public class DbContextFactory : IDesignTimeDbContextFactory<SalesDbContext>
    {
        public SalesDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SalesDbContext>();
            builder.UseSqlServer("Server=.\\SQLExpress;Database=Merp-ProjectManagement-ReadModel;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new SalesDbContext(builder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.Accountancy.QueryStack
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AccountancyDbContext>
    {
        public AccountancyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AccountancyDbContext>();
            builder.UseSqlServer("Server=.\\SQLExpress;Database=Merp-Accountancy-ReadModel;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new AccountancyDbContext(builder.Options);
        }
    }
}

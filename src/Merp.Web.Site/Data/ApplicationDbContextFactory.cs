using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Data
{
    public class ApplicationDbContextFactory: IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=.\\SQLExpress;Database=Merp;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new ApplicationDbContext(builder.Options);
        }
    }
}



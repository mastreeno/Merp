using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Merp.Web.Auth.Data
{
    public class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            builder.UseSqlServer(
                "Server=.\\SQLExpress;Database=Merp-Identity;Trusted_Connection=True;MultipleActiveResultSets=true",
                sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));

            var operationalStoreOptions = new OperationalStoreOptions();
            return new PersistedGrantDbContext(builder.Options, operationalStoreOptions);
        }
    }
}

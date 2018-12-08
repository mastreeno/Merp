using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Merp.Web.Auth.Data
{
    public class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            builder.UseSqlServer(
                "Server=.\\SQLExpress;Database=Merp-Identity;Trusted_Connection=True;MultipleActiveResultSets=true",
                sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));

            var configurationStoreOptions = new ConfigurationStoreOptions();
            return new ConfigurationDbContext(builder.Options, configurationStoreOptions);
        }
    }
}

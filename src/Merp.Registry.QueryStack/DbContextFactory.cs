using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Merp.Registry.QueryStack
{
    public class DbContextFactory : IDesignTimeDbContextFactory<RegistryDbContext>
    {
        public RegistryDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RegistryDbContext>();
            builder.UseSqlServer("Server=.\\SQLExpress;Database=Merp-Registry-ReadModel;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new RegistryDbContext(builder.Options);
        }
    }
}

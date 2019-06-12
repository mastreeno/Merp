using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Merp.Accountancy.Settings
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AccountancySettingsDbContext>
    {
        public AccountancySettingsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AccountancySettingsDbContext>();
            builder.UseSqlServer("Server=.\\SQLExpress;Database=Merp-Accountancy-ReadModel;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new AccountancySettingsDbContext(builder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Merp.Accountancy.Drafts
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AccountancyDraftsDbContext>
    {
        public AccountancyDraftsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AccountancyDraftsDbContext>();
            builder.UseSqlServer("Server=.\\SQLExpress;Database=Merp-Accountancy-ReadModel;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new AccountancyDraftsDbContext(builder.Options);
        }
    }
}

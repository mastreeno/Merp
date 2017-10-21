using Merp.Registry.QueryStack.Model;
using Microsoft.EntityFrameworkCore;

namespace Merp.Registry.QueryStack
{
    public class RegistryDbContext : DbContext
    {
        private readonly static string ContextName = "Registry";

        public DbSet<Party> Parties { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Company> Companies { get; set; }

        public RegistryDbContext() { }
        public RegistryDbContext(DbContextOptions<RegistryDbContext> options) : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Party());
            modelBuilder.ApplyConfiguration(new Person());
            modelBuilder.ApplyConfiguration(new Company());

            //foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    entity
            //        .Relational()
            //        .TableName = $"{ContextName}_{entity.ClrType.Name}";
            //}
        }
    }
}

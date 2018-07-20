using Merp.Sales.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Merp.Sales.QueryStack
{
    public class ProjectManagementDbContext : DbContext
    {
        private readonly static string ContextName = "Sales";

        public ProjectManagementDbContext() { }
        public ProjectManagementDbContext(DbContextOptions<ProjectManagementDbContext> options) : base(options){ }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Project());

            //foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    entity
            //        .Relational()
            //        .TableName = $"{ContextName}_{entity.ClrType.Name}";
            //}
        }
    }
}

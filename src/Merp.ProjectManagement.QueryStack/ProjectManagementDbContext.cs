using Merp.ProjectManagement.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Merp.ProjectManagement.QueryStack
{
    public class ProjectManagementDbContext : DbContext
    {
        private readonly static string ContextName = "ProjectManagement";

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

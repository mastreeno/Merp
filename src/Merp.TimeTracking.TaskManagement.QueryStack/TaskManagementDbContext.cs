using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Merp.TimeTracking.TaskManagement.QueryStack.Model;

namespace Merp.TimeTracking.TaskManagement.QueryStack
{
    public class TaskManagementDbContext : DbContext
    {
        private readonly static string ContextName = "TaskManagement";

        public TaskManagementDbContext() { }
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options){ }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new Party());

            //foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    entity
            //        .Relational()
            //        .TableName = $"{ContextName}_{entity.ClrType.Name}";
            //}
        }
    }
}

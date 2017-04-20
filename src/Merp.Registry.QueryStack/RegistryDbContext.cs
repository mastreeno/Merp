using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merp.Registry.QueryStack.Model;

namespace Merp.Registry.QueryStack
{
    public class RegistryDbContext : DbContext
    {
        private readonly static string ContextName = "Registry";

        public RegistryDbContext()
            : base("Merp-Registry-ReadModel")
        {
            
        }

        public RegistryDbContext(string connectionString)
            : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Types().Configure(entity => entity.ToTable($"{ContextName}_{entity.ClrType.Name}"));
        }

        public DbSet<Party> Parties { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Merp.Registry.QueryStack
{
    public class Database : IDatabase, IDisposable
    {
        private RegistryDbContext Context = null;

        public Database(RegistryDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IQueryable<Model.Party> Parties
        {
            get
            {
                return Context.Parties.AsNoTracking();
            }

        }
        public IQueryable<Model.Person> People
        {
            get
            {
                return Context.People.AsNoTracking();
            }

        }
        public IQueryable<Model.Company> Companies
        {
            get
            {
                return Context.Companies.AsNoTracking();
            }

        }
        public void Dispose()
        {
            if(Context!= null)
                Context.Dispose();
        }
    }
}

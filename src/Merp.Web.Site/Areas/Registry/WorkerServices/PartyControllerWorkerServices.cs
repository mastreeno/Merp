using Merp.Registry.QueryStack;
using Merp.Registry.QueryStack.Model;
using Merp.Web.Site.Areas.Registry.Models;
using Merp.Web.Site.Areas.Registry.Models.Party;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Merp.Web.Site.Areas.Registry.WorkerServices
{
    public class PartyControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; set; }

        public PartyControllerWorkerServices(IBus bus, IDatabase database)
        {
            if(bus==null)
                throw new ArgumentNullException(nameof(bus));
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            this.Bus = bus;
            this.Database = database;
        }

        public string GetDetailViewModel(Guid partyId)
        {
            if(Database.Parties.OfType<Company>().Where(p => p.OriginalId == partyId).Count()==1)
            {
                return "Company";
            }
            else if (Database.Parties.OfType<Person>().Where(p => p.OriginalId == partyId).Count() == 1)
            {
                return "Person";
            }
            else
            {
                return "Unknown";
            }
        }

        public IEnumerable<object> GetPartyNamesByPattern(string text)
        {
            var model = from p in Database.Parties
                        where p.DisplayName.StartsWith(text)
                        orderby p.DisplayName ascending
                        select new PartyInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName };
            return model;
        }

        public PartyInfo GetPartyInfoByPattern(int id)
        {
            var model = (from p in Database.Parties
                         where p.Id == id
                         select new PartyInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName }).Single();
            return model;
        }

        public IEnumerable<object> GetPersonNamesByPattern(string text)
        {
            var model = from p in Database.Parties.OfType<Person>()
                        where p.DisplayName.StartsWith(text)
                        orderby p.DisplayName ascending
                        select new PartyInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName };
            return model;
        }

        public PartyInfo GetPersonInfoByPattern(int id)
        {
            var model = (from p in Database.Parties.OfType<Person>()
                         where p.Id == id
                         select new PartyInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName }).Single();
            return model;
        }
        public IEnumerable<GetPartiesViewModel> GetParties(string query)
        {
            var model = from p in Database.Parties
                        orderby p.DisplayName ascending
                        select new GetPartiesViewModel { id = p.Id, uid = p.OriginalId, name = p.DisplayName };
            if(!string.IsNullOrEmpty(query) && query!="undefined")
            {
                model = model.Where(p => p.name.StartsWith(query));
            }
            model = model.Take(50);
            return model;
        }
    }
}
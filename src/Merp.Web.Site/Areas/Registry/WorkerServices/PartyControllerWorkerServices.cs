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
            this.Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
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


        public IEnumerable<GetPartiesViewModel> GetParties(string query)
        {
            var model = from p in Database.Parties
                        orderby p.DisplayName ascending
                        select new GetPartiesViewModel { id = p.Id, uid = p.OriginalId, name = p.DisplayName };
            if(!string.IsNullOrEmpty(query) && query!="undefined")
            {
                model = model.Where(p => p.name.StartsWith(query));
            }
            model = model.Take(20);
            return model;
        }
    }
}
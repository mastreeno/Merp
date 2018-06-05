using Merp.Registry.QueryStack;
using Merp.Registry.QueryStack.Model;
using Merp.Web.Site.Areas.Registry.Models;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry.WorkerServices
{
    public class ApiControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; set; }

        public ApiControllerWorkerServices(IBus bus, IDatabase database)
        {
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IEnumerable<object> GetPartyNamesByPattern(string text)
        {
            var model = from p in Database.Parties
                        where p.DisplayName.StartsWith(text)
                        orderby p.DisplayName ascending
                        select new PartyInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName };
            return model;
        }

        public PartyInfo GetPartyInfoById(int id)
        {
            var model = (from p in Database.Parties
                         where p.Id == id
                         select new PartyInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName }).Single();
            return model;
        }

        public PartyInfo GetPartyInfoById(Guid id)
        {
            var model = (from p in Database.Parties
                         where p.OriginalId == id
                         select new PartyInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName }).Single();
            return model;
        }

        public IEnumerable<object> GetPersonNamesByPattern(string text)
        {
            var model = from p in Database.People
                        where p.DisplayName.StartsWith(text)
                        orderby p.DisplayName ascending
                        select new PartyInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName };
            return model;
        }

        public PartyInfo GetPersonInfoByPattern(int id)
        {
            var model = (from p in Database.People
                         where p.Id == id
                         select new PartyInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName }).Single();
            return model;
        }
    }
}

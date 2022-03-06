using Merp.Registry.CommandStack.Commands;
using Merp.Registry.QueryStack;
using Merp.Registry.QueryStack.Model;
using Merp.Registry.Web.Models.Party;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Merp.Registry.Web.WorkerServices
{
    public class PartyControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; private set; }
        public IHttpContextAccessor ContextAccessor { get; private set; }

        public PartyControllerWorkerServices(IBus bus, IDatabase database, IHttpContextAccessor contextAccessor)
        {
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            Database = database ?? throw new ArgumentNullException(nameof(database));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public SearchModel SearchParties(string query, string partyType, string city, string postalCode, string orderBy, string orderDirection, int page, int size)
        {
            var parties = Database.Parties.NotUnlisted();
            parties = ApplyPartyTypeFilter(parties, partyType);
            parties = ApplyCityFilter(parties, city);
            parties = ApplyPostalCodeFilter(parties, postalCode);
            parties = ApplyOrdering(parties, orderBy, orderDirection);

            int skip = (page - 1) * size;

            var partyViewModels = parties.Select(
                p => new SearchModel.PartyDescriptor
                {
                    Id = p.Id,
                    Uid = p.OriginalId,
                    Name = p.DisplayName,
                    PhoneNumber = p.PhoneNumber,
                    NationalIdentificationNumber = p.NationalIdentificationNumber,
                    VatIndex = p.VatIndex,
                    PartyType = p.Type.ToString()
                });

            partyViewModels = ApplyNameFilter(partyViewModels, query);
            int totalNumberOfParties = partyViewModels.Count();

            partyViewModels = partyViewModels.Skip(skip).Take(size);
            return new SearchModel
            {
                TotalNumberOfParties = totalNumberOfParties,
                Parties = partyViewModels.ToList()
            };
        }

        public async Task UnlistParty(Guid partyId)
        {
            var userId = GetCurrentUserId();

            var party = Database.Parties.Single(p => p.OriginalId == partyId);
            var unlistDate = DateTime.Today;
            switch (party.Type)
            {
                case Party.PartyType.Company:
                    var unlistCompanyCmd = new UnlistCompanyCommand(userId, partyId, unlistDate);
                    await Bus.Send(unlistCompanyCmd);
                    break;
                case Party.PartyType.Person:
                    var unlistPersonCmd = new UnlistPersonCommand(userId, partyId, unlistDate);
                    await Bus.Send(unlistPersonCmd);
                    break;
                default:
                    return;
            }
        }

        public IEnumerable<PartyInfoModel> GetPartyInfoByPattern(string text)
        {
            var model = from p in Database.Parties
                        where p.DisplayName.StartsWith(text)
                        orderby p.DisplayName ascending
                        select new PartyInfoModel { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName };

            return model;
        }

        public PartyInfoModel GetPartyInfoById(Guid partyId)
        {
            var model = (from p in Database.Parties
                         where p.OriginalId == partyId
                         orderby p.DisplayName ascending
                         select new PartyInfoModel { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName }
                         ).Single();

            return model;
        }

        #region Helper Methods
        private Guid GetCurrentUserId()
        {
            var userId = ContextAccessor.HttpContext.User.FindFirstValue("sub");
            return Guid.Parse(userId);
        }

        private static IQueryable<Party> ApplyPartyTypeFilter(IQueryable<Party> parties, string partyType)
        {
            if ("person".Equals(partyType, StringComparison.OrdinalIgnoreCase))
            {
                return parties.Where(p => p.Type == Party.PartyType.Person);
            }
            if ("company".Equals(partyType, StringComparison.OrdinalIgnoreCase))
            {
                return parties.Where(p => p.Type == Party.PartyType.Company);
            }
            return parties;
        }

        private static IQueryable<Party> ApplyOrdering(IQueryable<Party> parties, string orderBy, string orderDirection)
        {
            if ("name".Equals(orderBy, StringComparison.OrdinalIgnoreCase))
            {
                parties = "desc".Equals(orderDirection, StringComparison.OrdinalIgnoreCase)
                    ? parties.OrderByDescending(p => p.DisplayName)
                    : parties.OrderBy(p => p.DisplayName);
            }
            else
            {
                parties = parties.OrderBy(p => p.DisplayName);
            }

            return parties;
        }

        private static IQueryable<Party> ApplyCityFilter(IQueryable<Party> parties, string city)
        {
            if (!string.IsNullOrEmpty(city) && city != "undefined")
            {
                parties = parties.Where(p => p.LegalAddress.City.Contains(city));
            }

            return parties;
        }

        private static IQueryable<Party> ApplyPostalCodeFilter(IQueryable<Party> parties, string postalCode)
        {
            if (!string.IsNullOrWhiteSpace(postalCode) && postalCode != "undefined")
            {
                parties = parties.Where(p => p.LegalAddress.PostalCode.Contains(postalCode));
            }

            return parties;
        }

        private static IQueryable<SearchModel.PartyDescriptor> ApplyNameFilter(IQueryable<SearchModel.PartyDescriptor> partyViewModels, string query)
        {
            if (!string.IsNullOrEmpty(query) && query != "undefined")
            {
                partyViewModels = partyViewModels.Where(p => p.Name.Contains(query));
            }

            return partyViewModels;
        }

        #endregion
    }
}

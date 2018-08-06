using Merp.Registry.CommandStack.Commands;
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
            if(Database.Companies.Where(p => p.OriginalId == partyId).Count()==1)
            {
                return "Company";
            }
            else if (Database.People.Where(p => p.OriginalId == partyId).Count() == 1)
            {
                return "Person";
            }
            else
            {
                return "Unknown";
            }
        }


        public GetPartiesViewModel GetParties(string query, string partyType, string city, string postalCode, string orderBy, string orderDirection, int page, int size)
        {
            var parties = Database.Parties.NotUnlisted();
            parties = ApplyPartyTypeFilter(parties, partyType);
            parties = ApplyCityFilter(parties, city);
            parties = ApplyPostalCodeFilter(parties, postalCode);
            parties = ApplyOrdering(parties, orderBy, orderDirection);

            int skip = (page - 1) * size;

            var partyViewModels = parties.Select(
                p => new GetPartiesViewModel.PartyDescriptor {
                    id = p.Id,
                    uid = p.OriginalId,
                    name = p.DisplayName,
                    PhoneNumber = p.PhoneNumber,
                    NationalIdentificationNumber = p.NationalIdentificationNumber,
                    VatIndex = p.VatIndex
                }
                );
            partyViewModels = ApplyNameFilter(partyViewModels, query);
            int totalNumberOfParties = partyViewModels.Count();

            partyViewModels = partyViewModels.Skip(skip).Take(size);
            return new GetPartiesViewModel
            {
                TotalNumberOfParties = totalNumberOfParties,
                Parties = partyViewModels.ToList()
            };
        }

        public void UnlistParty(Guid partyId)
        {
            var party = Database.Parties.Single(p => p.OriginalId == partyId);
            var unlistDate = DateTime.Today;
            switch (party.Type)
            {
                case Party.PartyType.Company:
                    var unlistCompanyCmd = new UnlistCompanyCommand(partyId, unlistDate);
                    Bus.Send(unlistCompanyCmd);
                    break;
                case Party.PartyType.Person:
                    var unlistPersonCmd = new UnlistPersonCommand(partyId, unlistDate);
                    Bus.Send(unlistPersonCmd);
                    break;
                default:
                    return;
            }
        }

        #region Helper Methods

        private static IQueryable<Party> ApplyPartyTypeFilter(IQueryable<Party> parties, string partyType)
        {
            if("person".Equals(partyType, StringComparison.OrdinalIgnoreCase))
            {
                return parties.Where(p => p.Type == Party.PartyType.Person);
            }
            if ("company".Equals(partyType, StringComparison.OrdinalIgnoreCase))
            {
                return parties.Where(p => p.Type== Party.PartyType.Company);
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

        private static IQueryable<GetPartiesViewModel.PartyDescriptor> ApplyNameFilter(IQueryable<GetPartiesViewModel.PartyDescriptor> partyViewModels, string query)
        {
            if (!string.IsNullOrEmpty(query) && query != "undefined")
            {
                partyViewModels = partyViewModels.Where(p => p.name.Contains(query));
            }

            return partyViewModels;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Merp.Registry.QueryStack;
using Merp.Registry.QueryStack.Model;

namespace Merp.Registry.Web.App.Pages
{
    public partial class Index
    {      
        [Inject] IDatabase Database { get; set; }
        public SearchResult Model = new();
        public SearchParameters Params = new();

        async Task Search()
        {            
            var parties = Database.Parties.NotUnlisted();          

            parties = Params.PartyType switch
            {
                "person" => parties.Where(p => p.Type == Party.PartyType.Person),
                "company" => parties.Where(p => p.Type == Party.PartyType.Company),
                _ => parties
            };

            if (!string.IsNullOrEmpty(Params.PartyName))
                parties = parties.Where(p => p.DisplayName.Contains(Params.PartyName));

            if (!string.IsNullOrEmpty(Params.CityName))
                parties = parties.Where(p => p.LegalAddress.City.Contains(Params.CityName));

            if (!string.IsNullOrWhiteSpace(Params.PostalCode))
                parties = parties.Where(p => p.LegalAddress.PostalCode.Contains(Params.PostalCode));

            parties = Params.Order switch
            {
                SearchParameters.SortOrder.Ascending => parties.OrderBy(p => p.DisplayName),
                SearchParameters.SortOrder.Descending => parties.OrderByDescending(p => p.DisplayName),
                _ => parties
            };

            int skip = (Params.PageIndex - 1) * Params.PageSize;

            var partyViewModels = parties.Select(
                p => new SearchResult.PartyDescriptor
                {
                    Id = p.Id,
                    Uid = p.OriginalId,
                    Name = p.DisplayName,
                    PhoneNumber = p.PhoneNumber,
                    NationalIdentificationNumber = p.NationalIdentificationNumber,
                    VatIndex = p.VatIndex,
                    PartyType = p.Type.ToString()
                });

            int totalNumberOfParties = partyViewModels.Count();
            partyViewModels = partyViewModels.Skip(skip).Take(Params.PageSize);
            
            Model.TotalNumberOfParties = totalNumberOfParties;
            Model.Parties = await partyViewModels.ToListAsync();
        }

        public class SearchResult
        {
            public IEnumerable<PartyDescriptor> Parties { get; set; }

            public int TotalNumberOfParties { get; set; }

            public class PartyDescriptor
            {
                public int Id { get; set; }

                public Guid Uid { get; set; }

                public string Name { get; set; }

                public string PhoneNumber { get; set; }

                public string NationalIdentificationNumber { get; set; }

                public string VatIndex { get; set; }

                public string PartyType { get; set; }
            }
        }

        public class SearchParameters
        {
            public int PageIndex = 1;
            public int PageSize = 15;

            public string CityName;
            public string PartyName;
            public string PartyType;
            public string PostalCode;
            public SortOrder Order = SortOrder.Ascending;

            public enum SortOrder
            {
                Ascending,
                Descending
            }
        }
    }
}

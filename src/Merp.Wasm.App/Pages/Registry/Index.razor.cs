using Merp.Wasm.App.Http;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Merp.Wasm.App.Pages.Registry
{
    public partial class Index
    {
        [Inject] RegistryPrivateApiHttpClient Http { get; set; }
        private ViewModel Model = new ViewModel();
        private SearchParameters Params = new SearchParameters();

        async Task Search()
        {
            string url = $"api/Party/Search?query={Params.PartyName}&partyType={Params.PartyType}&city={Params.CityName}&postalCode={Params.PostalCode}&orderBy={""}&orderDirection={""}&page={Params.PageIndex}&size={Params.PageSize}";
            Model = await Http.Http.GetFromJsonAsync<ViewModel>(url);
        }

        public class SearchParameters
        {
            public string CityName;
            public string PartyName;
            public string PartyType;
            public string PostalCode;
            public SortOrder Order = SortOrder.Ascending;
            public int PageIndex = 1;
            public int PageSize = 20;
        }

        public class ViewModel
        {
            public IEnumerable<PartyDescriptor> Parties { get; set; } = new List<PartyDescriptor>();

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

        public enum SortOrder
        {
            Ascending,
            Descending
        }
    }
}

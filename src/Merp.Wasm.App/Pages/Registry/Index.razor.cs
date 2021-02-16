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
        private RegistryPrivateApiHttpClient.SearchModel Model = new();
        private SearchParameters Params = new();

        async Task Search()
        {
            Model = await Http.SearchAsync(Params.PartyName, Params.PartyType, Params.CityName, Params.PostalCode, Params.PageIndex, Params.PageSize);
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

        public enum SortOrder
        {
            Ascending,
            Descending
        }
    }
}

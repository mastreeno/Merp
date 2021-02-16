using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace Merp.Wasm.App.Http
{
    public class RegistryPrivateApiHttpClient
    {
        public HttpClient Http { get; private set; }

        public RegistryPrivateApiHttpClient(HttpClient client)
        {
            Http = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<SearchModel> SearchAsync(string partyName, string partyType, string cityName, string postalCode, int pageIndex, int pageSize)
        {
            string url = $"api/Party/Search?query={partyName}&partyType={partyType}&city={cityName}&postalCode={postalCode}&orderBy={""}&orderDirection={""}&page={pageIndex}&size={pageSize}";
            return await this.Http.GetFromJsonAsync<SearchModel>(url);
        }


        public class SearchModel
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Merp.Wasm.App.Http
{
    public class RegistryInternalApiHttpClient
    {
        public HttpClient Http { get; private set; }

        public RegistryInternalApiHttpClient(HttpClient client)
        {
            Http = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<Country[]> GetCountriesAsync()
        {
            return await Http.GetFromJsonAsync<Country[]>("api/Countries/GetCountries");
        }

        public async Task<GetPartyInfoByVatNumberModel> GetPartyInfoByVatNumberAsync(string vatNumber, string countryCode)
        {
            try
            {
                var url = $"api/Person/LookupByVatNumber?vatNumber={vatNumber}&countryCode={countryCode}";
                return await Http.GetFromJsonAsync<GetPartyInfoByVatNumberModel>(url);
            }
            catch
            {
                return null;
            }
        }


        public class Country
        {
            public string Code { get; set; }
            public string DisplayName { get; set; }
        }

        public class GetPartyInfoByVatNumberModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string VatNumber { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Province { get; set; }
            public string Country { get; set; }
        }
    }
}

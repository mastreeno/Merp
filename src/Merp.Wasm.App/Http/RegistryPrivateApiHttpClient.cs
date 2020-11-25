using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        //public async Task<WeatherForecast[]> GetForecastsAsync()
        //{
        //    return await _client.GetJsonAsync<WeatherForecast[]>("weatherforecast");
        //}
    }
}

using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Merp.Web.Http
{
    public abstract class IdentityServerHttpClient
    {
        public HttpClient Client { get; private set; }

        private readonly string _authorityBaseUrl;

        private readonly string _clientId;

        private readonly string _clientSecret;

        public IdentityServerHttpClient(HttpClient client, string authorityBaseUrl, string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(authorityBaseUrl))
            {
                throw new ArgumentException("value cannot be empty", nameof(authorityBaseUrl));
            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentException("value cannot be empty", nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentException("value cannot be empty", nameof(clientSecret));
            }

            Client = client ?? throw new ArgumentNullException(nameof(client));
            _authorityBaseUrl = authorityBaseUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        protected virtual async Task ConfigureAccessToken(string apiName)
        {
            if (string.IsNullOrWhiteSpace(apiName))
            {
                throw new ArgumentException("value cannot be empty", nameof(apiName));
            }

            var discoveryClient = await DiscoveryClient.GetAsync(_authorityBaseUrl);
            if (discoveryClient.IsError)
            {
                throw new InvalidOperationException(discoveryClient.Error);
            }

            var tokenClient = new TokenClient(discoveryClient.TokenEndpoint, _clientId, _clientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(apiName);
            if (tokenResponse.IsError)
            {
                throw new InvalidOperationException(tokenResponse.Error);
            }

            Client.SetBearerToken(tokenResponse.AccessToken);
        }
    }
}

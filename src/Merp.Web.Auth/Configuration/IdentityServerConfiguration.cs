using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Auth.Configuration
{
    public class IdentityServerConfiguration
    {
        private readonly IDictionary<string, ClientConfiguration> _clients;

        private readonly IEnumerable<ApiResourceConfiguration> _apiResources;

        public IdentityServerConfiguration(IDictionary<string, ClientConfiguration> clients, IEnumerable<ApiResourceConfiguration> apiResources)
        {
            _clients = clients;
            _apiResources = apiResources;
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public IEnumerable<ApiResource> GetApiResources()
        {
            return _apiResources.Select(a => new ApiResource(a.Name, a.DisplayName));
        }

        public IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientId = _clients["Merp.Web.App"].ClientId,
                    ClientName = _clients["Merp.Web.App"].ClientName,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AllowedCorsOrigins = { _clients["Merp.Web.App"].Endpoint },
                    RedirectUris = { _clients["Merp.Web.App"].RedirectUri },
                    PostLogoutRedirectUris = { _clients["Merp.Web.App"].PostLogoutUri },
                    ClientSecrets =
                    {
                        new Secret(_clients["Merp.Web.App"].Secret.Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }.Concat(_apiResources.Select(a => a.Name)).ToArray()
                }
            };
        }
    }
}

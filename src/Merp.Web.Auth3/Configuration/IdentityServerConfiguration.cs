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
                },
                new Client
                {
                    ClientId = _clients["Merp.WebAssembly.App"].ClientId,
                    ClientName = _clients["Merp.WebAssembly.App"].ClientName,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AllowedCorsOrigins = { _clients["Merp.WebAssembly.App"].Endpoint },
                    RedirectUris = { _clients["Merp.WebAssembly.App"].RedirectUri },
                    PostLogoutRedirectUris = { _clients["Merp.WebAssembly.App"].PostLogoutUri },
                    ClientSecrets =
                    {
                        new Secret(_clients["Merp.WebAssembly.App"].Secret.Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }.Concat(_apiResources.Select(a => a.Name)).ToArray()
                },
                new Client
                {
                    ClientId = _clients["Merp.Martin.Web.Alexa"].ClientId,
                    ClientName = _clients["Merp.Martin.Web.Alexa"].ClientName,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = { "https://layla.amazon.com/api/skill/link/M21TL5SCAFP3CZ", "https://alexa.amazon.co.jp/api/skill/link/M21TL5SCAFP3CZ", "https://pitangui.amazon.com/api/skill/link/M21TL5SCAFP3CZ" },
                    ClientSecrets =
                    {
                        new Secret(_clients["Merp.Martin.Web.Alexa"].Secret.Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }.Concat(_apiResources.Select(a => a.Name)).ToArray()
                },
                new Client
                {
                    ClientId = _clients["Merp.Martin.Web.BotFramework"].ClientId,
                    ClientName = _clients["Merp.Martin.Web.BotFramework"].ClientName,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = { _clients["Merp.Martin.Web.BotFramework"].RedirectUri },
                    ClientSecrets =
                    {
                        new Secret(_clients["Merp.Martin.Web.BotFramework"].Secret.Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }.Concat(_apiResources.Select(a => a.Name)).ToArray()
                },
                new Client
                {
                    ClientId = _clients["Merp.Import.Cli"].ClientId,
                    ClientName = _clients["Merp.Import.Cli"].ClientName,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    // AllowedCorsOrigins = { _clients["Merp.Web.App"].Endpoint },
                    // RedirectUris = { _clients["Merp.Web.App"].RedirectUri },
                    // PostLogoutRedirectUris = { _clients["Merp.Web.App"].PostLogoutUri },
                    ClientSecrets =
                    {
                        new Secret(_clients["Merp.Import.Cli"].Secret.Sha256())
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

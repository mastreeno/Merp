// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Merp.Web.Auth
{
    public class Config
    {
        private readonly IDictionary<string, ClientConfiguration> _clients;

        private readonly IEnumerable<ApiResourceConfiguration> _apiResources;

        public Config(IConfiguration configuration)
        {
            _clients = new Dictionary<string, ClientConfiguration>();
            _apiResources = new List<ApiResourceConfiguration>();

            configuration.GetSection("ClientConfigurations").Bind(_clients);
            configuration.GetSection("ApiResourceConfigurations").Bind(_apiResources);
        }

        public IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                   };

        public IEnumerable<ApiScope> ApiScopes => _apiResources.Select(a => new ApiScope(a.Name, a.DisplayName));
        public IEnumerable<ApiResource> ApiResouces => _apiResources.Select(a => new ApiResource(a.Name, a.DisplayName) { Scopes = new[] { a.Name } });

        public IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = _clients["Merp.Web.App"].ClientId,
                    ClientName = _clients["Merp.Web.App"].ClientName,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AllowedCorsOrigins = { _clients["Merp.Web.App"].Endpoint },
                    Enabled = true,
                    RequireConsent = false,
                    RequireClientSecret = false,
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
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    AllowedCorsOrigins = { _clients["Merp.WebAssembly.App"].Endpoint },
                    Enabled = true,
                    PostLogoutRedirectUris = { _clients["Merp.WebAssembly.App"].PostLogoutUri },
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RedirectUris = { _clients["Merp.WebAssembly.App"].RedirectUri },
                    RequirePkce = true,
                    //ClientSecrets =
                    //{
                    //    new Secret(_clients["Merp.WebAssembly.App"].Secret.Sha256())
                    //},
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
                //// m2m client credentials flow client
                //new Client
                //{
                //    ClientId = "m2m.client",
                //    ClientName = "Client Credentials Client",

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //    AllowedScopes = { "scope1" }
                //},

                //// interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //},
            };

        public class ClientConfiguration
        {
            public string ClientId { get; set; }

            public string ClientName { get; set; }

            public string Endpoint { get; set; }

            public string RedirectUri { get; set; }

            public string PostLogoutUri { get; set; }

            public string Secret { get; set; }
        }

        public class ApiResourceConfiguration
        {
            public string Name { get; set; }

            public string DisplayName { get; set; }
        }
    }
}
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Merp.Wasm.App.Http
{
    public static class IServiceCollectionExtensions
    {
        public static void AddRegistryPrivateApiHttpClient(this IServiceCollection services, string address)
        {
            if (string.IsNullOrWhiteSpace(address)) address = "https://localhost:44373";

            services.AddHttpClient<RegistryPrivateApiHttpClient>(h =>
            {
                h.BaseAddress = new Uri($"{address}/api");
            })
            .AddHttpMessageHandler(sp =>
            {
                var handler = sp.GetService<AuthorizationMessageHandler>()
                    .ConfigureHandler(
                        authorizedUrls: new[] { address },
                        scopes: new[] { "merp.registry.api" });
                return handler;
            });

            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                            .CreateClient("merp.registry.api"));
        }

        public static void AddRegistryInternalApiHttpClient(this IServiceCollection services, string address)
        {
            if (string.IsNullOrWhiteSpace(address)) address = "https://localhost:44385";

            services.AddHttpClient<RegistryInternalApiHttpClient>(h =>
            {
                h.BaseAddress = new Uri($"{address}/api");
            })
            .AddHttpMessageHandler(sp =>
             {
                 var handler = sp.GetService<AuthorizationMessageHandler>()
                     .ConfigureHandler(
                         authorizedUrls: new[] { address },
                         scopes: new[] { "merp.registry.api.internal" });
                 return handler;
            });

            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                    .CreateClient("merp.registry.api.internal"));
        }
    }
}

using Merp.Wasm.App.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Wasm.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services
                .AddHttpClient<RegistryPrivateApiHttpClient>(h => h.BaseAddress = new Uri("https://localhost:44373/api"))
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { "https://localhost:44373" },
                            scopes: new[] { "merp.registry.api" });
                    return handler;
                });
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                            .CreateClient("merp.registry.api"));

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("Local", options.ProviderOptions);

                options.ProviderOptions.DefaultScopes.Add("merp.auth.api");
                options.ProviderOptions.DefaultScopes.Add("merp.accountancy.api");
                options.ProviderOptions.DefaultScopes.Add("merp.accountancy.api.internal");
                options.ProviderOptions.DefaultScopes.Add("merp.accountancy.api.public");
                options.ProviderOptions.DefaultScopes.Add("merp.registry.api");
                options.ProviderOptions.DefaultScopes.Add("merp.registry.api.internal");
                options.ProviderOptions.DefaultScopes.Add("merp.registry.api.public");
                options.ProviderOptions.DefaultScopes.Add("merp.timetracking.api");
            });

            await builder.Build().RunAsync();
        }
    }
}

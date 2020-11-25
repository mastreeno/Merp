using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Auth
{
    public static class IServiceCollectionExtensions
    {
        public static void RegisterClientsCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("WebApp", policy =>
                {
                    policy.WithOrigins(configuration["ClientEndpoints:WebApp"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
                options.AddPolicy("WasmApp", policy =>
                {
                    policy.WithOrigins(configuration["ClientEndpoints:WasmApp"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}

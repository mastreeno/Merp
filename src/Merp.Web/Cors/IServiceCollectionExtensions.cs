using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Cors
{
    public static class IServiceCollectionExtensions
    {
        public static void RegisterClientsCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                //options.AddPolicy("WebApp", policy =>
                //{
                //    policy.WithOrigins(configuration["ClientEndpoints:WebApp"])
                //        .AllowAnyHeader()
                //        .AllowAnyMethod();
                //});
                //options.AddPolicy("WasmApp", policy =>
                //{
                //    policy.WithOrigins(configuration["ClientEndpoints:WasmApp"])
                //        .AllowAnyHeader()
                //        .AllowAnyMethod();
                //});
                options.AddPolicy("MerpClientApps", policy =>
                {
                    policy.WithOrigins(configuration["ClientEndpoints:WasmApp"], configuration["ClientEndpoints:WebApp"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}

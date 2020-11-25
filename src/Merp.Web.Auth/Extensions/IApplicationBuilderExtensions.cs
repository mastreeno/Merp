using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Auth
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseClientsCors(this IApplicationBuilder app)
        {
            app.UseCors("WebApp");
            app.UseCors("WasmApp");
        }
    }
}

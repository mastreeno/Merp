using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Cors
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseClientsCors(this IApplicationBuilder app)
        {
            app.UseCors("MerpClientApps");
            //app.UseCors("WebApp");
            //app.UseCors("WasmApp");
            //app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}

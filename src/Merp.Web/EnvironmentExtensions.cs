using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; 

namespace Merp.Web
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool IsAWS(this IWebHostEnvironment env)
        {
            return env.EnvironmentName.ToUpper().Contains("AWS");
        }
        public static bool IsAzure(this IWebHostEnvironment env)
        {
            return env.EnvironmentName.ToUpper().Contains("AZURE");
        }
        public static bool IsDevelopment(this IWebHostEnvironment env)
        {
            return env.EnvironmentName.ToUpper().Contains("DEVELOPMENT");
        }
        public static bool IsOnPremises(this IWebHostEnvironment env)
        {
            return env.EnvironmentName.ToUpper().Contains("ONPREMISES");
        }
    }
}

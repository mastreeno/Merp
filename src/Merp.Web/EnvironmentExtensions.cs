using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; 

namespace Merp.Web
{
    public static class EnvironmentExtensions
    {
        public static bool IsDevelopment(this IWebHostEnvironment env)
        {
            return env.EnvironmentName.ToUpper().Contains("DEVELOPMENT");
        }
        public static bool IsAzureCosmosDB(this IWebHostEnvironment env)
        {
            return env.EnvironmentName.ToUpper().Contains("AZURECOSMOSDB");
        }
        public static bool IsAzureMongoDB(this IWebHostEnvironment env)
        {
            return env.EnvironmentName.ToUpper().Contains("AZUREMONGODB");
        }
        public static bool IsAWS(this IWebHostEnvironment env)
        {
            return env.EnvironmentName.ToUpper().Contains("AWS");
        }
        public static bool IsOnPremises(this IWebHostEnvironment env)
        {
            return env.EnvironmentName.ToUpper().Contains("ONPREMISES");
        }
    }
}

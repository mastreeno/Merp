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
        public static bool IsAzureCosmosDB(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Contains("AzureCosmosDB");
        }
        public static bool IsAzureMongoDB(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Contains("AzureMongoDB");
        }
        public static bool IsOnPremises(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Contains("OnPremises");
        }
    }
}

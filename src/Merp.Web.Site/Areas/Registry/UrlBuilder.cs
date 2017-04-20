using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Registry
{
    public class UrlBuilder
    {
        private const string areaName = "Registry";

        public static class Registry
        {
            public static string CompanyInfo(Guid companyId)
            {
                return $"/{areaName}/Company/Info/{companyId}";
            }

            public static string PersonInfo(Guid companyId)
            {
                return $"/{areaName}/Person/Info/{companyId}";
            }

            public static string Search()
            {
                return $"/{areaName}/Party/Search";
            }
        }
    }
}

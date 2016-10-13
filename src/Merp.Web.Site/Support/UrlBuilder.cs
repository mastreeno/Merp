using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Support
{
    public static class UrlBuilder
    {
        public static class Accountancy
        {

        }

        public static class Registry
        {
            public static string CompanyInfo(Guid companyId)
            {
                return $"/Registry/Company/Info/{companyId}";
            }

            public static string PersonInfo(Guid companyId)
            {
                return $"/Registry/Person/Info/{companyId}";
            }

            public static string Search()
            {
                return "/Registry/Party/Search";
            }
        }
    }
}
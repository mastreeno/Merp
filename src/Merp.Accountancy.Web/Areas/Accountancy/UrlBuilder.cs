using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Accountancy
{
    public static class UrlBuilder
    {
        private const string areaName = "Accountancy";

        public static string JobOrderGetListUrl()
        {
            return $"/{areaName}/Api/GetList";
        }
    }
}

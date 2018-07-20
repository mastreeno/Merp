using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Sales.Web.Areas.Sales
{
    public class UrlBuilder
    {
        private const string areaName = "Sales";

        public static string ProjectGetListUrl()
        {
            return $"/{areaName}/Api/GetList";
        }

        public static string ProjectDetail()
        {
            return string.Empty;
        }

        public static string RegisterProgect()
        {
            return $"/{areaName}/Project/Register";
        }
    }
}

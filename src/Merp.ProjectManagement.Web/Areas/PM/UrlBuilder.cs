using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.ProjectManagement.Web.Areas.PM
{
    public class UrlBuilder
    {
        private const string areaName = "PM";

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

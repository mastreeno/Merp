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

        public static string JobOrderGetListUrl()
        {
            return $"/{areaName}/Api/GetList";
        }
    }
}

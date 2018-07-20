using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Sales.Web.Areas.Sales.Models.Project
{
    public class SearchModel
    {
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }

        public ProjectState State { get; set; }
        public enum ProjectState
        {
            Registered,
            Current,
            Completed
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.ProjectManagement.Models.Home
{
    public class BalanceViewModel
    {
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }

        public enum Scale
        {
            Daily,
            Weekly,
            Monthly,
            Quarterly,
            Yearly
        }
    }
}
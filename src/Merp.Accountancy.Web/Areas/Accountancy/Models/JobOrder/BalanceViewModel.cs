using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
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
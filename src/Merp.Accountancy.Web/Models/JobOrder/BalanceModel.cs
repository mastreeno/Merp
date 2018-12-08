using System;

namespace Merp.Accountancy.Web.Models.JobOrder
{
    public class BalanceModel
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

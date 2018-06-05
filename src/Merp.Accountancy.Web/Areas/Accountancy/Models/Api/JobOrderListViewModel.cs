using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Accountancy.Models.Api
{
    public class JobOrderListViewModel
    {
        public class JobOrder
        {
            public int Id { get; set; }
            public Guid OriginalId { get; set; }
            public Guid CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string Name { get; set; }
            public string Number { get; set; }
            public bool IsCompleted { get; set; }
            public string Tenant { get; set; }

            public string DisplayName
            {
                get
                {
                    return $"{this.Tenant} - {this.Number} - {this.Name}";
                }
            }
        }

        public IEnumerable<JobOrder> List { get; set; }

        public JobOrderListViewModel()
        {
            this.List = new List<JobOrder>();
        }
    }
}

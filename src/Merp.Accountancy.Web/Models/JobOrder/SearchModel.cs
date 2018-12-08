using System;
using System.Collections.Generic;

namespace Merp.Accountancy.Web.Models.JobOrder
{
    public class SearchModel
    {
        public IEnumerable<JobOrderDescriptor> JobOrders { get; set; }

        public int TotalNumberOfJobOrders { get; set; }

        public class JobOrderDescriptor
        {
            public int Id { get; set; }
            public Guid OriginalId { get; set; }
            public Guid CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string Name { get; set; }
            public string Number { get; set; }
            public bool IsCompleted { get; set; }
        }
    }
}

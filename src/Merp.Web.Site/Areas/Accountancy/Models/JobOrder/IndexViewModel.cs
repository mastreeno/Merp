using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Merp.Web.Site.Areas.Accountancy.Models.JobOrder
{
    public class IndexViewModel
    {
        public bool CurrentOnly { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public Guid SelectedCustomerId { get; set; }

        public class CustomerInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class JobOrder
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
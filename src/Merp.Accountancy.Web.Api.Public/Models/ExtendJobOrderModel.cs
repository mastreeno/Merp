using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public.Models
{
    public class ExtendJobOrderModel
    {
        public Guid JobOrderId { get; set; }

        public DateTime NewDueDate { get; set; }

        public decimal Price { get; set; }

        public Guid UserId { get; set; }
    }
}

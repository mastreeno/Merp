using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public.Models
{
    public class MarkJobOrderAsCompletedModel
    {
        public DateTime DateOfCompletion { get; set; }

        public Guid JobOrderId { get; set; }

        public Guid UserId { get; set; }
    }
}

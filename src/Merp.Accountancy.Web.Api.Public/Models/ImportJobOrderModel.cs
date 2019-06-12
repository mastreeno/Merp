using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public.Models
{
    public class ImportJobOrderModel
    {
        public Guid? ContactPersonId { get; set; }

        public string Currency { get; set; }

        public PartyInfo Customer { get; set; }

        public DateTime DateOfRegistration { get; set; }

        public DateTime DateOfStart { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsTimeAndMaterial { get; set; }

        public Guid JobOrderId { get; set; }

        public string JobOrderName { get; set; }

        public string JobOrderNumber { get; set; }

        public Guid ManagerId { get; set; }

        public decimal? Price { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public Guid UserId { get; set; }

        public class PartyInfo
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}

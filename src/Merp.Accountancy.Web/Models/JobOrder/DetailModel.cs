using System;

namespace Merp.Accountancy.Web.Models.JobOrder
{
    public class DetailModel
    {
        public Guid JobOrderId { get; set; }

        public string JobOrderNumber { get; set; }

        public string JobOrderName { get; set; }

        public Guid CustomerId { get; set; }

        public Guid? ContactPersonId { get; set; }

        public Guid ManagerId { get; set; }

        public decimal Price { get; set; }

        public DateTime DateOfStart { get; set; }

        public DateTime DueDate { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public decimal Balance { get; set; }
    }
}

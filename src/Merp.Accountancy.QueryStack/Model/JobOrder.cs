using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Model
{
    public class JobOrder
    {
        public int Id { get; set; }

        [Required]
        public Guid OriginalId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        [Required]
        public Guid ManagerId { get; set; }

        public string ManagerName { get; set; }

        public DateTime DateOfStart { get; set; }

        public DateTime? DateOfCompletion { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsFixedPrice { get; set; }

        public bool IsTimeAndMaterial { get; set; }

        public string Description { get; set; }

        [Required]
        public string Number { get; set; }

        public string PurchaseOrderNumber { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public JobOrder()
        {
            IsFixedPrice = false;
            IsTimeAndMaterial = false;
        }
    }
}

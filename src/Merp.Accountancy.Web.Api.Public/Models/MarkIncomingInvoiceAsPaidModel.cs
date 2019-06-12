using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public.Models
{
    public class MarkIncomingInvoiceAsPaidModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid InvoiceId { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }
    }
}

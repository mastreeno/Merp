using Merp.Web.Site.Areas.Registry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.Invoice
{
    public class IssueViewModel
    {
        public PartyInfo Customer { get; set; }

        [DisplayName("Date of issue")]
        [Required]
        public DateTime Date { get; set; }

        [DisplayName("Amount")]
        [Required]
        public decimal Amount { get; set; }

        [DisplayName("Taxes")]
        [Required]
        public decimal Taxes { get; set; }

        [DisplayName("Total price")]
        [Required]
        public decimal TotalPrice { get; set; }

        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }

        [DisplayName("Payment terms")]
        public string PaymentTerms { get; set; }

        [DisplayName("PO #")]
        public string PurchaseOrderNumber { get; set; }
    }
}
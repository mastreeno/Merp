using Merp.Web.Site.Areas.Registry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.Accountancy.Models.Invoice
{
    public class IssueViewModel
    {
        public PartyInfo Customer { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public decimal Taxes { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public string Description { get; set; }
        public string PaymentTerms { get; set; }
        public string PurchaseOrderNumber { get; set; }
    }
}
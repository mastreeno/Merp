using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models.Invoice
{
    public class RegisterIncomingInvoiceModel
    {
        public IncomingDocumentTypes Type { get; set; }

        public PartyInfo Supplier { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public string Currency { get; set; }

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

        public bool VatIncluded { get; set; }

        public IEnumerable<InvoiceLineItemModel> LineItems { get; set; }

        public IEnumerable<InvoicePriceByVatModel> PricesByVat { get; set; }

        public IEnumerable<NonTaxableItemModel> NonTaxableItems { get; set; }
    }
}

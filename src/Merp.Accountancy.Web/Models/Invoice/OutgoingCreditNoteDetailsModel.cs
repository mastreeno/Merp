using System;
using System.Collections.Generic;

namespace Merp.Accountancy.Web.Models.Invoice
{
    public class OutgoingCreditNoteDetailsModel
    {
        public BillingInfo Customer { get; set; }

        public BillingInfo Supplier { get; set; }

        public string CreditNoteNumber { get; set; }

        public DateTime Date { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public decimal Taxes { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalToPay { get; set; }

        public string Description { get; set; }

        public string PaymentTerms { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public bool IsOverdue { get; set; }

        public IEnumerable<InvoiceLineItemModel> LineItems { get; set; }

        public IEnumerable<NonTaxableItemModel> NonTaxableItems { get; set; }
        
        public ProvidenceFundModel ProvidenceFund { get; set; }

        public WithholdingTaxModel WithholdingTax { get; set; }
    }
}

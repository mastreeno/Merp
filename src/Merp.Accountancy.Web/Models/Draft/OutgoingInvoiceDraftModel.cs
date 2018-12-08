using System;
using System.Collections.Generic;

namespace Merp.Accountancy.Web.Models.Draft
{
    public class OutgoingInvoiceDraftModel
    {
        public PartyInfo Customer { get; set; }

        public DateTime? Date { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public decimal Taxes { get; set; }

        public decimal TotalPrice { get; set; }

        public string Description { get; set; }

        public string PaymentTerms { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public bool VatIncluded { get; set; }

        public IEnumerable<DraftLineItemModel> LineItems { get; set; }

        public IEnumerable<DraftPriceByVatModel> PricesByVat { get; set; }

        public IEnumerable<NonTaxableItemModel> NonTaxableItems { get; set; }
    }
}

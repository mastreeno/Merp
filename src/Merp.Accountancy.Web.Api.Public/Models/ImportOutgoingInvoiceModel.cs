using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public.Models
{
    public class ImportOutgoingInvoiceModel
    {
        public string Currency { get; set; }

        public PartyInfo Customer { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime InvoiceDate { get; set; }

        public Guid InvoiceId { get; set; }

        public string InvoiceNumber { get; set; }

        public IEnumerable<InvoiceLineItem> LineItems { get; set; }

        public IEnumerable<NonTaxableItem> NonTaxableItems { get; set; }

        public string PaymentTerms { get; set; }

        public bool PricesAreVatIncluded { get; set; }

        public IEnumerable<InvoicePriceByVat> PricesByVat { get; set; }

        public string ProvidenceFundDescription { get; set; }

        public decimal? ProvidenceFundRate { get; set; }

        public decimal? ProvidenceFundAmount { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public PartyInfo Supplier { get; set; }

        public decimal TaxableAmount { get; set; }

        public decimal Taxes { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalToPay { get; set; }

        public Guid UserId { get; set; }

        public decimal? WithholdingTaxAmount { get; set; }

        public string WithholdingTaxDescription { get; set; }

        public decimal? WithholdingTaxRate { get; set; }

        public decimal? WithholdingTaxTaxableAmountRate { get; set; }

        public class PartyInfo
        {
            public string City { get; set; }

            public string Country { get; set; }

            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NationalIdentificationNumber { get; set; }

            public string PostalCode { get; set; }

            public string StreetName { get; set; }

            public string VatIndex { get; set; }
        }

        public class InvoiceLineItem
        {
            public string Code { get; set; }

            public string Description { get; set; }

            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal TotalPrice { get; set; }

            public decimal Vat { get; set; }

            public string VatDescription { get; set; }
        }

        public class InvoicePriceByVat
        {
            public decimal TaxableAmount { get; set; }

            public decimal VatRate { get; set; }

            public decimal VatAmount { get; set; }

            public decimal TotalPrice { get; set; }

            public decimal? ProvidenceFundAmount { get; set; }
        }

        public class NonTaxableItem
        {
            public string Description { get; set; }

            public decimal Amount { get; set; }
        }
    }
}

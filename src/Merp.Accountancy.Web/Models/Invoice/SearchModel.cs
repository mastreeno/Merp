using System;
using System.Collections.Generic;

namespace Merp.Accountancy.Web.Models.Invoice
{
    public class SearchModel
    {
        public IEnumerable<Invoice> Invoices { get; set; }

        public int TotalNumberOfInvoices { get; set; }

        public class Invoice
        {
            public Guid Uid { get; set; }
            public string DocumentType { get; set; }
            public string Number { get; set; }
            public DateTime Date { get; set; }
            public DateTime? DueDate { get; set; }
            public string SupplierName { get; set; }
            public string CustomerName { get; set; }
            public decimal TotalPrice { get; set; }
            public string Currency { get; set; }
        }

        public enum InvoiceKind
        {
            Any,
            IncomingInvoices,
            OutgoingInvoices,
            OutgoingCreditNotes,
            IncomingCreditNotes
        }

        public enum InvoiceState
        {
            Outstanding,
            Overdue,
            Paid
        }
    }
}

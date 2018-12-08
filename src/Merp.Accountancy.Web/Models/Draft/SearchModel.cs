using System;
using System.Collections.Generic;

namespace Merp.Accountancy.Web.Models.Draft
{
    public class SearchModel
    {
        public IEnumerable<Draft> Drafts { get; set; }

        public int TotalNumberOfDrafts { get; set; }

        public class Draft
        {
            public Guid Id { get; set; }
            public DateTime? Date { get; set; }
            public string DocumentType { get; set; }
            public string CustomerName { get; set; }
            public decimal TotalPrice { get; set; }
            public string Currency { get; set; }
        }

        public enum DraftKind
        {
            Any,
            OutgoingInvoice,
            OutgoingCreditNote
        }
    }
}

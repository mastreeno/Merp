using System;
using System.Collections.Generic;

namespace Merp.Accountancy.Web.Models.JobOrder
{
    public class IncomingCreditNotesAssociatedToJobOrderModel
    {
        public IEnumerable<CreditNote> IncomingCreditNotes { get; set; }

        public class CreditNote
        {
            public string Number { get; set; }
            public DateTime DateOfIssue { get; set; }
            public decimal Price { get; set; }
            public string Currency { get; set; }
            public string SupplierName { get; set; }
        }
    }
}

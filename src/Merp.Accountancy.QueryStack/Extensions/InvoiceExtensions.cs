using Merp.Accountancy.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack
{
    public static class InvoiceExtensions
    {
        private static IQueryable<T> _NotAssociatedToAnyJobOrder<T>(this IQueryable<T> invoices) where T : Invoice
        {
            return invoices.Where(i => !i.JobOrderId.HasValue);
        }

        public static IQueryable<IncomingCreditNote> NotAssociatedToAnyJobOrder(this IQueryable<IncomingCreditNote> creditNotes)
        {
            return _NotAssociatedToAnyJobOrder(creditNotes);
        }

        public static IQueryable<IncomingInvoice> NotAssociatedToAnyJobOrder(this IQueryable<IncomingInvoice> invoices)
        {
            return _NotAssociatedToAnyJobOrder(invoices);
        }

        public static IQueryable<OutgoingInvoice> NotAssociatedToAnyJobOrder(this IQueryable<OutgoingInvoice> invoices)
        {
            return _NotAssociatedToAnyJobOrder(invoices);
        }

        public static IQueryable<OutgoingCreditNote> NotAssociatedToAnyJobOrder(this IQueryable<OutgoingCreditNote> creditNotes)
        {
            return _NotAssociatedToAnyJobOrder(creditNotes);
        }

        private static IQueryable<T> _PerJobOrder<T>(this IQueryable<T> invoices, Guid jobOrderId) where T : Invoice
        {
            return invoices.Where(i => i.JobOrderId == jobOrderId);
        }

        public static IQueryable<IncomingCreditNote> PerJobOrder(this IQueryable<IncomingCreditNote> creditNotes, Guid jobOrderId)
        {
            return _PerJobOrder(creditNotes, jobOrderId);
        }

        public static IQueryable<IncomingInvoice> PerJobOrder(this IQueryable<IncomingInvoice> invoices, Guid jobOrderId)
        {
            return _PerJobOrder(invoices, jobOrderId);
        }

        public static IQueryable<OutgoingCreditNote> PerJobOrder(this IQueryable<OutgoingCreditNote> creditNotes, Guid jobOrderId)
        {
            return _PerJobOrder(creditNotes, jobOrderId);
        }

        public static IQueryable<OutgoingInvoice> PerJobOrder(this IQueryable<OutgoingInvoice> invoices, Guid jobOrderId)
        {
            return _PerJobOrder(invoices, jobOrderId);
        }

        public static IQueryable<Invoice> Outstanding(this IQueryable<Invoice> invoices)
        {
            return invoices.Where(i => !i.IsPaid);
        }

        public static IQueryable<Invoice> Overdue(this IQueryable<Invoice> invoices)
        {
            var today = DateTime.Now;
            return invoices
                    .Outstanding()
                    .Where(i => i.DueDate < today);
        }
    }
}

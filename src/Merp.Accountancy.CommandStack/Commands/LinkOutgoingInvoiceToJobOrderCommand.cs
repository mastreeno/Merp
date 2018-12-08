using System;
using MementoFX.Domain;
using Merp.Domain;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class LinkOutgoingInvoiceToJobOrderCommand : MerpCommand
    {
        public Guid JobOrderId { get; set; }

        public Guid InvoiceId { get; set; }

        [Timestamp]
        public DateTime DateOfLink { get; set; }

        public decimal Amount { get; set; }

        public LinkOutgoingInvoiceToJobOrderCommand(Guid userId, Guid invoiceId, Guid jobOrderId, DateTime dateOfLink, decimal amount)
            : base(userId)
        {
            InvoiceId = invoiceId;
            JobOrderId = jobOrderId;
            DateOfLink = dateOfLink;
            Amount = amount;
        }
    }
}

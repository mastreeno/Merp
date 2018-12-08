using MementoFX.Domain;
using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Commands
{
    public class LinkOutgoingCreditNoteToJobOrderCommand : MerpCommand
    {
        public Guid JobOrderId { get; set; }

        public Guid CreditNoteId { get; set; }

        [Timestamp]
        public DateTime DateOfLink { get; set; }

        public decimal Amount { get; set; }

        public LinkOutgoingCreditNoteToJobOrderCommand(Guid userId, Guid creditNoteId, Guid jobOrderId, DateTime dateOfLink, decimal amount)
            : base(userId)
        {
            CreditNoteId = creditNoteId;
            JobOrderId = jobOrderId;
            DateOfLink = dateOfLink;
            Amount = amount;
        }
    }
}

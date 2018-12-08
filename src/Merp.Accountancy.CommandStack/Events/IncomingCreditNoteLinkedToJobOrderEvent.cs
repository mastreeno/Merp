using MementoFX.Domain;
using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Events
{
    public class IncomingCreditNoteLinkedToJobOrderEvent : MerpDomainEvent
    {
        public Guid CreditNoteId { get; set; }

        public Guid JobOrderId { get; set; }

        [Timestamp]
        public DateTime DateOfLink { get; set; }

        public decimal Amount { get; set; }

        public IncomingCreditNoteLinkedToJobOrderEvent(Guid creditNoteId, Guid jobOrderId, DateTime DateOfLink, decimal amount, Guid userId)
            : base(userId)
        {
            CreditNoteId = creditNoteId;
            JobOrderId = jobOrderId;
            this.DateOfLink = DateOfLink;
            Amount = amount;
        }
    }
}

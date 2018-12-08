using MementoFX.Domain;
using Merp.Domain;
using System;

namespace Merp.Accountancy.CommandStack.Events
{
    public class OutgoingCreditNoteLinkedToJobOrderEvent : MerpDomainEvent
    {
        public Guid CreditNoteId { get; set; }

        public Guid JobOrderId { get; set; }

        [Timestamp]
        public DateTime DateOfLink { get; set; }

        public decimal Amount { get; set; }

        public OutgoingCreditNoteLinkedToJobOrderEvent(Guid creditNoteId, Guid jobOrderId, DateTime dateOfLink, decimal amount, Guid userId)
            : base(userId)
        {
            CreditNoteId = creditNoteId;
            JobOrderId = jobOrderId;
            DateOfLink = dateOfLink;
            Amount = amount;
        }
    }
}

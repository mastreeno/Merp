using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento.Persistence;
using Memento;
using Memento.Domain;
using Merp.Accountancy.CommandStack.Events;

namespace Merp.Accountancy.CommandStack.Model
{
    public abstract class JobOrder : Aggregate,
        IApplyEvent<IncomingInvoiceLinkedToJobOrderEvent>,
        IApplyEvent<OutgoingInvoiceLinkedToJobOrderEvent>
    {
        public Guid CustomerId { get; protected set; }
        public Guid ManagerId { get; protected set; }
        public string Name { get; protected set; }
        public string Number { get; protected set; }
        public DateTime DateOfStart { get; protected set; }
        public DateTime? DateOfCompletion { get; protected set; }
        public bool IsCompleted { get; protected set; }
        public string PurchaseOrderNumber { get; protected set; }
        public string Description { get; protected set; }

        public decimal Balance { get; private set; }

        public class CustomerInfo
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }

            public CustomerInfo(Guid id, string name)
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("Id cannot be empty", nameof(id));
                if(string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be null or empty", nameof(name));
                Id = id;
                Name = name;
            }
        }

        public class ManagerInfo
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }

            public ManagerInfo(Guid id, string name)
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Id cannot be empty", "id");
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name cannot be null or empty", "name");
                }
                Id = id;
                Name = name;
            }
        }

        public static decimal CalculateBalance(IEventStore es, Guid jobOrderId)
        {
            return CalculateBalance(es, jobOrderId, DateTime.Now);
        }

        public static decimal CalculateBalance(IEventStore es, Guid jobOrderId, DateTime balanceDate)
        {
            if (es == null)
            {
                throw new ArgumentNullException("es");
            }
            var outgoingInvoicesIds = es
                .Find<OutgoingInvoiceLinkedToJobOrderEvent>(e => e.JobOrderId == jobOrderId && e.TimeStamp <= balanceDate)
                .Select(e => e.InvoiceId)
                .ToArray();
            var earnings = es
                .Find<OutgoingInvoiceIssuedEvent>(e => outgoingInvoicesIds.Contains(e.InvoiceId))
                .Sum(e => e.Amount);

            var incomingInvoicesIds = es
                .Find<IncomingInvoiceLinkedToJobOrderEvent>(e => e.JobOrderId == jobOrderId && e.TimeStamp <= balanceDate)
                .Select(e => e.InvoiceId)
                .ToArray();
            var costs = es
                .Find<IncomingInvoiceRegisteredEvent>(e => incomingInvoicesIds.Any(id => e.InvoiceId == id))
                .Sum(e => e.Amount);

            decimal balance = earnings - costs;

            return balance;
        }

        public void ApplyEvent([AggregateId(nameof(IncomingInvoiceLinkedToJobOrderEvent.JobOrderId))] IncomingInvoiceLinkedToJobOrderEvent evt)
        {
            this.Balance -= evt.Amount;
        }

        public void ApplyEvent([AggregateId(nameof(OutgoingInvoiceLinkedToJobOrderEvent.JobOrderId))] OutgoingInvoiceLinkedToJobOrderEvent evt)
        {
            this.Balance += evt.Amount;
        }

        /// <summary>
        /// Associate an incoming invoice to the current Job Order
        /// </summary>
        /// <param name="eventStore">The event store</param>
        /// <param name="invoiceId">The Id of the Invoice to be associated to the current Job Order</param>
        /// <exception cref="InvalidOperationException">Thrown if the specified invoiceId refers to an invoice which has already been associated to a Job Order</exception>
        public void LinkIncomingInvoice(IEventStore eventStore, Guid invoiceId, DateTime dateOfLink, decimal amount)
        {
            if (this.IsCompleted)
                throw new InvalidOperationException("Can't relate new costs to a completed job order");
            var count = eventStore.Find<IncomingInvoiceLinkedToJobOrderEvent>(e => e.InvoiceId == invoiceId).Count();
            if(count>0)
                throw new InvalidOperationException("The specified invoice is already associated to a Job Order.");
            var @event = new IncomingInvoiceLinkedToJobOrderEvent(invoiceId, this.Id, dateOfLink, amount);
            RaiseEvent(@event);
        }

        /// <summary>
        /// Associate an outgoing invoice to the current Job Order
        /// </summary>
        /// <param name="eventStore">The event store</param>
        /// <param name="invoiceId">The Id of the Invoice to be associated to the current Job Order</param>
        /// <exception cref="InvalidOperationException">Thrown if the specified invoiceId refers to an invoice which has already been associated to a Job Order</exception>
        public void LinkOutgoingInvoice(IEventStore eventStore, Guid invoiceId, DateTime dateOfLink, decimal amount)
        {
            if (this.IsCompleted)
                throw new InvalidOperationException("Can't relate new revenues to a completed job order");
            var count = eventStore.Find<OutgoingInvoiceLinkedToJobOrderEvent>(e => e.InvoiceId == invoiceId).Count();
            if (count > 0)
                throw new InvalidOperationException("The specified invoice is already associated to a Job Order.");
            var @event = new OutgoingInvoiceLinkedToJobOrderEvent(invoiceId, this.Id, dateOfLink, amount);
            RaiseEvent(@event);
        }
    }
}

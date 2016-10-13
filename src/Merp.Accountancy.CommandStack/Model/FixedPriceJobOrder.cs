using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;
using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.CommandStack.Services;

namespace Merp.Accountancy.CommandStack.Model
{
    public class FixedPriceJobOrder : JobOrder,
        IApplyEvent<FixedPriceJobOrderExtendedEvent>,
        IApplyEvent<FixedPriceJobOrderCompletedEvent>,
        IApplyEvent<FixedPriceJobOrderRegisteredEvent>
    {
        public DateTime DueDate { get; private set; }
        public PositiveMoney Price { get; private set; }
        protected FixedPriceJobOrder()
        {
            
        }

        public void ApplyEvent([AggregateId(nameof(FixedPriceJobOrderExtendedEvent.JobOrderId))] FixedPriceJobOrderExtendedEvent evt)
        {
            this.DueDate = evt.NewDueDate;
            this.Price = new PositiveMoney(evt.Price, this.Price.Currency);
        }

        public void ApplyEvent([AggregateId(nameof(FixedPriceJobOrderCompletedEvent.JobOrderId))] FixedPriceJobOrderCompletedEvent evt)
        {
            this.DateOfCompletion = evt.DateOfCompletion;
            this.IsCompleted = true;
        }

        public void ApplyEvent([AggregateId(nameof(FixedPriceJobOrderRegisteredEvent.JobOrderId))] FixedPriceJobOrderRegisteredEvent evt)
        {
            Id = evt.JobOrderId;
            CustomerId = evt.CustomerId;
            ManagerId = evt.ManagerId;
            Price = new PositiveMoney(evt.Price, evt.Currency);
            DateOfStart= evt.DateOfStart;
            DueDate=evt.DueDate;
            Name = evt.JobOrderName;
            Number = evt.JobOrderNumber; 
            IsCompleted = false;
            PurchaseOrderNumber = evt.PurchaseOrderNumber;
            Description = evt.Description;
        }

        public void Extend(DateTime newDueDate, decimal price)
        {
            if (this.IsCompleted)
                throw new InvalidOperationException("Can't extend a completed job order.");
            if (this.DueDate > newDueDate)
                throw new ArgumentException("A job order length cannot be reduced.", nameof(newDueDate));

            var @event = new FixedPriceJobOrderExtendedEvent(
                this.Id, 
                newDueDate,
                price
            );
            RaiseEvent(@event);
        }

        public void MarkAsCompleted(DateTime dateOfCompletion)
        {
            if(this.DateOfStart > dateOfCompletion)
                throw new ArgumentException("The date of completion cannot precede the date of start.", nameof(dateOfCompletion));
            if(this.IsCompleted)
                throw new InvalidOperationException("The Job Order has already been marked as completed");

            var @event = new FixedPriceJobOrderCompletedEvent(
                this.Id,
                dateOfCompletion
            );
            RaiseEvent(@event);
        }

        public class Factory
        {
            public static FixedPriceJobOrder CreateNewInstance(IJobOrderNumberGenerator jobOrderNumberGenerator, Guid customerId, Guid managerId, decimal price, string currency, DateTime dateOfStart, DateTime dueDate, string name, string purchaseOrderNumber, string description)
            {
                if (jobOrderNumberGenerator == null)
                    throw new ArgumentNullException(nameof(jobOrderNumberGenerator));
                if (price < 0)
                    throw new ArgumentException("The price must be zero or higher", nameof(price));
                if (string.IsNullOrWhiteSpace(currency))
                    throw new ArgumentException("The currency must me specified", nameof(currency));
                if (dueDate < dateOfStart)
                    throw new ArgumentException("The due date cannot precede the starting date", nameof(dueDate));
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("The job order must have a name", nameof(name));

                var @event = new FixedPriceJobOrderRegisteredEvent(
                    Guid.NewGuid(),
                    customerId,
                    managerId, 
                    price,
                    currency,
                    dateOfStart,
                    dueDate,
                    name,
                    jobOrderNumberGenerator.Generate(),
                    purchaseOrderNumber,
                    description
                    );
                var jobOrder = new FixedPriceJobOrder();
                jobOrder.RaiseEvent(@event);
                return jobOrder;
            }
        }
    }
}

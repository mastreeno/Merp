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
    public class TimeAndMaterialJobOrder : JobOrder,
        IApplyEvent<TimeAndMaterialJobOrderExtendedEvent>,
        IApplyEvent<TimeAndMaterialJobOrderCompletedEvent>,
        IApplyEvent<TimeAndMaterialJobOrderRegisteredEvent>
    {
        public PositiveMoney Value { get; private set; }

        public DateTime? DateOfExpiration { get; private set; }

        protected TimeAndMaterialJobOrder()
        {
            
        }

        public void ApplyEvent([AggregateId(nameof(TimeAndMaterialJobOrderExtendedEvent.JobOrderId))] TimeAndMaterialJobOrderExtendedEvent evt)
        {
            this.DateOfExpiration = evt.NewDateOfExpiration;
            this.Value = new PositiveMoney(evt.Value, this.Value.Currency);
        }

        public void ApplyEvent([AggregateId(nameof(TimeAndMaterialJobOrderCompletedEvent.JobOrderId))] TimeAndMaterialJobOrderCompletedEvent evt)
        { 
            this.DateOfCompletion = evt.DateOfCompletion;
            this.IsCompleted = true;        
        }

        public void ApplyEvent([AggregateId(nameof(TimeAndMaterialJobOrderRegisteredEvent.JobOrderId))] TimeAndMaterialJobOrderRegisteredEvent evt)
        {
            Id = evt.JobOrderId;
            CustomerId = evt.CustomerId;
            ManagerId = evt.ManagerId;
            Value = new PositiveMoney(evt.Value, evt.Currency);
            DateOfStart = evt.DateOfStart;
            DateOfExpiration = evt.DateOfExpiration;
            Name = evt.JobOrderName;
            Number = evt.JobOrderNumber; 
            IsCompleted = false;
            PurchaseOrderNumber = evt.PurchaseOrderNumber;
            Description = evt.Description;
        }

        public void Extend(DateTime? newDateOfExpiration, decimal value)
        {
            var @event = new TimeAndMaterialJobOrderExtendedEvent(
                this.Id,
                newDateOfExpiration,
                value
            );
            RaiseEvent(@event);
        }

        public void MarkAsCompleted(DateTime dateOfCompletion)
        {
            if (this.DateOfStart > dateOfCompletion)
                throw new ArgumentException("The date of completion cannot precede the date of start.", nameof(dateOfCompletion));
            if (this.IsCompleted)
                throw new InvalidOperationException("The Job Order has already been marked as completed");

            var @event = new TimeAndMaterialJobOrderCompletedEvent(
                this.Id,
                dateOfCompletion
            );
            RaiseEvent(@event);
        }

        public class Factory
        {
            public static TimeAndMaterialJobOrder CreateNewInstance(IJobOrderNumberGenerator jobOrderNumberGenerator, Guid customerId, Guid managerId, decimal value, string currency, DateTime dateOfStart, DateTime? dateOfExpiration, string name, string purchaseOrderNumber, string description)
            {
                if (jobOrderNumberGenerator == null)
                    throw new ArgumentNullException(nameof(jobOrderNumberGenerator));
                if (value < 0)
                    throw new ArgumentException("The value must be zero or higher", nameof(value));
                if (string.IsNullOrWhiteSpace(currency))
                    throw new ArgumentException("The currency must me specified", nameof(currency));
                if (dateOfExpiration.HasValue && dateOfExpiration.Value < dateOfStart)
                    throw new ArgumentException("The date of expiration cannot precede the starting date", nameof(dateOfExpiration));
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("The job order must have a name", nameof(name));

                var @event = new TimeAndMaterialJobOrderRegisteredEvent(
                    Guid.NewGuid(),
                    customerId,
                    managerId,
                    value,
                    currency,
                    dateOfStart,
                    dateOfExpiration,
                    name,
                    jobOrderNumberGenerator.Generate(),
                    purchaseOrderNumber,
                    description
                    );
                var jobOrder = new TimeAndMaterialJobOrder(); 
                jobOrder.RaiseEvent(@event);
                return jobOrder;
            }
        }
    }
}

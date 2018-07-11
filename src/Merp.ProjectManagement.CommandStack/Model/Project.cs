using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX.Persistence;
using MementoFX;
using MementoFX.Domain;
using Merp.ProjectManagement.CommandStack.Events;
using Merp.ProjectManagement.CommandStack.Services;
using Merp.Domain.Model;

namespace Merp.ProjectManagement.CommandStack.Model
{
    public class Project : Aggregate,
        IApplyEvent<ProjectExtendedEvent>,
        IApplyEvent<ProjectCompletedEvent>,
        IApplyEvent<ProjectRegisteredEvent>
    {
        public Guid CustomerId { get; protected set; }
        public Guid? ContactPersonId { get; protected set; }
        public Guid ManagerId { get; protected set; }

        public string Name { get; protected set; }

        public string Number { get; protected set; }

        public DateTime DateOfStart { get; protected set; }

        public DateTime? DateOfCompletion { get; protected set; }

        public DateTime DueDate { get; private set; }

        public PositiveMoney Price { get; private set; }

        public bool IsCompleted { get; protected set; }

        public string CustomerPurchaseOrderNumber { get; protected set; }

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

        public void ApplyEvent(ProjectExtendedEvent evt)
        {
            this.DueDate = evt.NewDueDate;
            this.Price = new PositiveMoney(evt.Price, this.Price.Currency);
        }

        public void ApplyEvent(ProjectCompletedEvent evt)
        {
            this.DateOfCompletion = evt.DateOfCompletion;
            this.IsCompleted = true;
        }

        public void ApplyEvent(ProjectRegisteredEvent evt)
        {
            Id = evt.ProjectId;
            CustomerId = evt.CustomerId;
            ContactPersonId = evt.ContactPersonId;
            ManagerId = evt.ManagerId;
            DateOfStart = evt.DateOfStart;
            DueDate = evt.DueDate;
            Name = evt.ProjectName;
            Number = evt.ProjectNumber;
            IsCompleted = false;
            CustomerPurchaseOrderNumber = evt.PurchaseOrderNumber;
            Description = evt.Description;
            if (evt.Price.HasValue)
                Price = new PositiveMoney(evt.Price.Value, evt.Currency);
            else
                evt.Price = null;
        }

        public void Extend(DateTime newDueDate, decimal price)
        {
            if (this.IsCompleted)
                throw new InvalidOperationException("Can't extend a completed job order.");
            if (this.DueDate > newDueDate)
                throw new ArgumentException("A job order length cannot be reduced.", nameof(newDueDate));

            var @event = new ProjectExtendedEvent(
                this.Id,
                newDueDate,
                price
            );
            RaiseEvent(@event);
        }

        public void MarkAsCompleted(DateTime dateOfCompletion)
        {
            if (this.DateOfStart > dateOfCompletion)
                throw new ArgumentException("The date of completion cannot precede the date of start.", nameof(dateOfCompletion));
            if (this.IsCompleted)
                throw new InvalidOperationException("The Job Order has already been marked as completed");

            var @event = new ProjectCompletedEvent(
                this.Id,
                dateOfCompletion
            );
            RaiseEvent(@event);
        }

        public class Factory
        {
            public static Project RegisterNew(IProjectNumberGenerator jobOrderNumberGenerator, Guid customerId, string customerName, Guid? contactPersonId, Guid managerId, decimal? price, string currency, DateTime dateOfStart, DateTime dueDate, bool isTimeAndMaterial, string name, string purchaseOrderNumber, string description)
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

                var @event = new ProjectRegisteredEvent(
                    Guid.NewGuid(),
                    customerId,
                    customerName,
                    contactPersonId,
                    managerId,
                    price,
                    currency,
                    DateTime.Now,
                    dateOfStart,
                    dueDate,
                    isTimeAndMaterial,
                    name,
                    jobOrderNumberGenerator.Generate(),
                    purchaseOrderNumber,
                    description
                    );
                var jobOrder = new Project();
                jobOrder.RaiseEvent(@event);
                return jobOrder;
            }

            public static Project Import(Guid jobOrderId, string jobOrderNumber, Guid customerId, string customerName, Guid? contactPersonId, Guid managerId, decimal? price, string currency, DateTime dateOfRegistration, DateTime dateOfStart, DateTime dueDate, bool isTimeAndMaterial, string name, string purchaseOrderNumber, string description)
            {
                if (string.IsNullOrWhiteSpace(jobOrderNumber))
                    throw new ArgumentNullException(nameof(jobOrderNumber), "A job order number must be provided");
                if (price < 0 && price != -1)
                    throw new ArgumentException("The price must be zero or higher", nameof(price));
                if (string.IsNullOrWhiteSpace(currency))
                    throw new ArgumentException("The currency must me specified", nameof(currency));
                if (dueDate < dateOfStart)
                    throw new ArgumentException("The due date cannot precede the starting date", nameof(dueDate));
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("The job order must have a name", nameof(name));

                var @event = new ProjectRegisteredEvent(
                    jobOrderId,
                    customerId,
                    customerName,
                    contactPersonId,
                    managerId,
                    price,
                    currency,
                    dateOfRegistration,
                    dateOfStart,
                    dueDate,
                    isTimeAndMaterial,
                    name,
                    jobOrderNumber,
                    purchaseOrderNumber,
                    description
                    );
                var jobOrder = new Project();
                jobOrder.RaiseEvent(@event);
                return jobOrder;
            }
        }
    }
}

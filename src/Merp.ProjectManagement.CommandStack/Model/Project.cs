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

        public DateTime? DateOfStart { get; protected set; }

        public DateTime? DateOfCompletion { get; protected set; }

        public DateTime? DueDate { get; private set; }

        public Money Price { get; private set; }

        public bool IsCompleted { get; protected set; }

        public string CustomerPurchaseOrderNumber { get; protected set; }

        public string Description { get; protected set; }

        public decimal Balance { get; private set; }

        public void ApplyEvent(ProjectExtendedEvent evt)
        {
            this.DueDate = evt.NewDueDate;
            this.Price = new Money(evt.Price, this.Price.Currency);
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
            CustomerPurchaseOrderNumber = evt.CustomerPurchaseOrderNumber;
            Description = evt.Description;
            Price = new Money(evt.Price, evt.Currency);
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
            public static Project RegisterNew(IProjectNumberGenerator projectNumberGenerator, Guid customerId, Guid? contactPersonId, Guid managerId, Money price, DateTime? dateOfStart, DateTime? dueDate, bool isTimeAndMaterial, string customerPurchaseOrderNumber, string name, string description)
            {
                if (projectNumberGenerator == null)
                    throw new ArgumentNullException(nameof(projectNumberGenerator));
                if (dueDate < dateOfStart)
                    throw new ArgumentException("The due date cannot precede the starting date", nameof(dueDate));
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("The project must have a name", nameof(name));

                var @event = new ProjectRegisteredEvent(
                    Guid.NewGuid(),
                    customerId,
                    contactPersonId,
                    managerId,
                    price.Amount,
                    price.Currency,
                    DateTime.Now,
                    dateOfStart,
                    dueDate,
                    isTimeAndMaterial,
                    name,
                    projectNumberGenerator.Generate(),
                    customerPurchaseOrderNumber,
                    description
                    );
                var jobOrder = new Project();
                jobOrder.RaiseEvent(@event);
                return jobOrder;
            }

            public static Project Import(Guid projectId, string projectNumber, Guid customerId, Guid? contactPersonId, Guid managerId, Money price, DateTime dateOfRegistration, DateTime? dateOfStart, DateTime? dueDate, bool isTimeAndMaterial, string name, string purchaseOrderNumber, string description)
            {
                if (string.IsNullOrWhiteSpace(projectNumber))
                    throw new ArgumentNullException(nameof(projectNumber), "A job order number must be provided");
                if (dueDate < dateOfStart)
                    throw new ArgumentException("The due date cannot precede the starting date", nameof(dueDate));
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("The job order must have a name", nameof(name));

                var @event = new ProjectRegisteredEvent(
                    projectId,
                    customerId,
                    contactPersonId,
                    managerId,
                    price.Amount,
                    price.Currency,
                    dateOfRegistration,
                    dateOfStart,
                    dueDate,
                    isTimeAndMaterial,
                    name,
                    projectNumber,
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;
using Merp.Registry.CommandStack.Events;

namespace Merp.Registry.CommandStack.Model
{
    public class Person : Party,
        IApplyEvent<PersonRegisteredEvent>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        
        protected Person()
        {

        }

        public void ApplyEvent(PersonRegisteredEvent evt)
        {
            this.Id = evt.PersonId;
            this.FirstName = evt.FirstName;
            this.LastName = evt.LastName;
        }

        public static class Factory
        {
            public static Person CreateNewEntry(string firstName, string lastName, DateTime? dateOfBirth)
            {
                if (string.IsNullOrWhiteSpace(firstName))
                    throw new ArgumentException("The first name must be specified", nameof(firstName));
                if (string.IsNullOrWhiteSpace(lastName))
                    throw new ArgumentException("The first name must be specified", nameof(lastName));

                var e = new PersonRegisteredEvent(Guid.NewGuid(), firstName, lastName);
                var p = new Person();
                p.RaiseEvent(e);
                return p;
            }
        }
    }
}

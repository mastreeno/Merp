using Memento.Persistence;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.CommandStack.Model;
using Rebus.Sagas;
using Rebus.Bus;
using System;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Sagas
{
    public class PersonSaga : Saga<PersonSaga.PersonSagaData>,
        IAmInitiatedBy<RegisterPersonCommand>
    {
        private readonly IRepository _repository;
        private readonly IBus _bus;

        public PersonSaga(IRepository repository, IBus bus)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));

            this._repository = repository;
            this._bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<PersonSagaData> config)
        {
            config.Correlate<RegisterPersonCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);
        }

        public Task Handle(RegisterPersonCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var person = Person.Factory.CreateNewEntry(message.FirstName, message.LastName, message.DateOfBirth);
                _repository.Save<Person>(person);
                this.Data.PersonId = person.Id;
            });
        }

        public class PersonSagaData : SagaData
        {
            public Guid PersonId { get; set; }
        }
    }
}

using Memento.Persistence;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.CommandStack.Model;
using Rebus.Sagas;
using Rebus.Bus;
using System;
using System.Threading.Tasks;
using Merp.Registry.CommandStack.Services;

namespace Merp.Registry.CommandStack.Sagas
{
    public class PersonSaga : Saga<PersonSaga.PersonSagaData>,
        IAmInitiatedBy<RegisterPersonCommand>,
        IAmInitiatedBy<ImportPersonCommand>
    {
        private readonly IRepository _repository;
        private readonly IBus _bus;
        private readonly IDefaultCountryResolver _defaultCountryResolver;

        public PersonSaga(IRepository repository, IBus bus, IDefaultCountryResolver defaultCountryResolver)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _defaultCountryResolver = defaultCountryResolver ?? throw new ArgumentNullException(nameof(defaultCountryResolver));
        }

        protected override void CorrelateMessages(ICorrelationConfig<PersonSagaData> config)
        {
            config.Correlate<ImportPersonCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);
            config.Correlate<RegisterPersonCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);
        }

        public Task Handle(RegisterPersonCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var person = Person.Factory.CreateNewEntry(message.FirstName, message.LastName, message.NationalIdentificationNumber, message.VatNumber);
                if (!string.IsNullOrWhiteSpace(message.Address) && !string.IsNullOrWhiteSpace(message.City))
                    person.SetAddress(message.Address, message.City, message.PostalCode, message.Province, !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry());
                _repository.Save<Person>(person);
                this.Data.PersonId = person.Id;
            });
        }

        public Task Handle(ImportPersonCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var person = Person.Factory.CreateNewEntryByImport(message.PersonId, message.FirstName, message.LastName, message.NationalIdentificationNumber, message.VatNumber);
                if (!!string.IsNullOrWhiteSpace(message.Address) && !string.IsNullOrWhiteSpace(message.City))
                    person.SetAddress(message.Address, message.City, message.PostalCode, message.Province, message.Country);
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

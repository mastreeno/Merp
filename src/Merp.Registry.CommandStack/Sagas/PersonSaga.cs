using MementoFX.Persistence;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.CommandStack.Model;
using Rebus.Sagas;
using Rebus.Bus;
using System;
using System.Threading.Tasks;
using Merp.Registry.CommandStack.Services;
using Rebus.Handlers;

namespace Merp.Registry.CommandStack.Sagas
{
    public class PersonSaga : Saga<PersonSaga.PersonSagaData>,
        IAmInitiatedBy<RegisterPersonCommand>,
        IAmInitiatedBy<ImportPersonCommand>,
        IHandleMessages<ChangePersonAddressCommand>,
        IHandleMessages<ChangePersonContactInfoCommand>
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

            config.Correlate<ChangePersonAddressCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);

            config.Correlate<ChangePersonContactInfoCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);
        }

        public async Task Handle(RegisterPersonCommand message)
        {
            var country = string.IsNullOrWhiteSpace(message.Address) || !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry();
            var person = Person.Factory.CreateNewEntry(message.FirstName, message.LastName, message.NationalIdentificationNumber, message.VatNumber, message.Address, message.City, message.PostalCode, message.Province, country, message.PhoneNumber, message.MobileNumber, message.FaxNumber, message.WebsiteAddress, message.EmailAddress, message.InstantMessaging);
            await _repository.SaveAsync(person);
            this.Data.PersonId = person.Id;
        }

        public async Task Handle(ImportPersonCommand message)
        {
            var country = string.IsNullOrWhiteSpace(message.Address) || !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry();
            var person = Person.Factory.CreateNewEntryByImport(message.PersonId,message.RegistrationDate, message.FirstName, message.LastName, message.NationalIdentificationNumber, message.VatNumber, message.Address, message.City, message.PostalCode, message.Province, country, message.PhoneNumber, message.MobileNumber, message.FaxNumber, message.WebsiteAddress, message.EmailAddress, message.InstantMessaging);
            await _repository.SaveAsync(person);
            this.Data.PersonId = person.Id;
        }

        public async Task Handle(ChangePersonAddressCommand message)
        {
            var person = _repository.GetById<Person>(message.PersonId);
            if (person.ShippingAddress == null || person.ShippingAddress.IsDifferentAddress(message.Address, message.City, message.PostalCode, message.Province, message.Country))
            {
                var effectiveDateTime = message.EffectiveDate;
                var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);
                person.ChangeAddress(message.Address, message.City, message.PostalCode, message.Province, !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry(), effectiveDate);
                await _repository.SaveAsync(person);
            }
        }

        public async Task Handle(ChangePersonContactInfoCommand message)
        {
            var person = _repository.GetById<Person>(message.PersonId);
            if (person.ContactInfo.PhoneNumber != message.PhoneNumber || person.ContactInfo.MobileNumber != message.MobileNumber || person.ContactInfo.FaxNumber != message.FaxNumber || person.ContactInfo.WebsiteAddress != message.WebsiteAddress || person.ContactInfo.EmailAddress != message.EmailAddress || person.ContactInfo.InstantMessaging != message.InstantMessaging)
            {
                person.SetContactInfo(message.PhoneNumber, message.MobileNumber, message.FaxNumber, message.WebsiteAddress, message.EmailAddress, message.InstantMessaging);
                await _repository.SaveAsync(person);
            }
        }

        public class PersonSagaData : SagaData
        {
            public Guid PersonId { get; set; }
        }
    }
}

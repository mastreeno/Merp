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
        IHandleMessages<ChangePersonLegalAddressCommand>,
        IHandleMessages<ChangePersonContactInfoCommand>,
        IHandleMessages<ChangePersonShippingAddressCommand>,
        IHandleMessages<ChangePersonBillingAddressCommand>,
        IHandleMessages<UnlistPersonCommand>
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

            config.Correlate<ChangePersonLegalAddressCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);

            config.Correlate<ChangePersonContactInfoCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);

            config.Correlate<ChangePersonShippingAddressCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);

            config.Correlate<ChangePersonBillingAddressCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);

            config.Correlate<UnlistPersonCommand>(
                message => message.PersonId,
                sagaData => sagaData.PersonId);
        }

        public async Task Handle(RegisterPersonCommand message)
        {
            var legalAddressCountry = string.IsNullOrWhiteSpace(message.LegalAddressAddress) || !string.IsNullOrWhiteSpace(message.LegalAddressCountry) ? message.LegalAddressCountry : _defaultCountryResolver.GetDefaultCountry();
            var shippingAddressCountry = string.IsNullOrWhiteSpace(message.ShippingAddressAddress) || !string.IsNullOrWhiteSpace(message.ShippingAddressCountry) ? message.ShippingAddressCountry : _defaultCountryResolver.GetDefaultCountry();
            var billingAddressCountry = string.IsNullOrWhiteSpace(message.BillingAddressAddress) || !string.IsNullOrWhiteSpace(message.BillingAddressCountry) ? message.BillingAddressCountry : _defaultCountryResolver.GetDefaultCountry();

            var person = Person.Factory.CreateNewEntry(message.FirstName, message.LastName, message.NationalIdentificationNumber, message.VatNumber, 
                message.LegalAddressAddress, message.LegalAddressCity, message.LegalAddressPostalCode, message.LegalAddressProvince, legalAddressCountry, 
                message.BillingAddressAddress, message.BillingAddressCity, message.BillingAddressPostalCode, message.BillingAddressPostalCode, billingAddressCountry,
                message.ShippingAddressAddress, message.ShippingAddressCity, message.ShippingAddressPostalCode, message.ShippingAddressProvince, shippingAddressCountry,
                message.PhoneNumber, message.MobileNumber, message.FaxNumber, message.WebsiteAddress, message.EmailAddress, message.InstantMessaging, message.UserId);
            await _repository.SaveAsync(person);
            this.Data.PersonId = person.Id;
        }

        public async Task Handle(ImportPersonCommand message)
        {
            var country = string.IsNullOrWhiteSpace(message.Address) || !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry();
            var person = Person.Factory.CreateNewEntryByImport(message.PersonId,message.RegistrationDate, message.FirstName, message.LastName, message.NationalIdentificationNumber, message.VatNumber, message.Address, message.City, message.PostalCode, message.Province, country, message.PhoneNumber, message.MobileNumber, message.FaxNumber, message.WebsiteAddress, message.EmailAddress, message.InstantMessaging, message.UserId);
            await _repository.SaveAsync(person);
            this.Data.PersonId = person.Id;
        }

        public async Task Handle(ChangePersonLegalAddressCommand message)
        {
            var person = _repository.GetById<Person>(message.PersonId);
            if (person.LegalAddress == null || person.LegalAddress.IsDifferentAddress(message.Address, message.City, message.PostalCode, message.Province, message.Country))
            {
                var effectiveDateTime = message.EffectiveDate;
                var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);
                person.ChangeLegalAddress(message.Address, message.City, message.PostalCode, message.Province, !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry(), effectiveDate, message.UserId);
                await _repository.SaveAsync(person);
            }
        }

        public async Task Handle(ChangePersonContactInfoCommand message)
        {
            var person = _repository.GetById<Person>(message.PersonId);
            if (person.ContactInfo.PhoneNumber != message.PhoneNumber || person.ContactInfo.MobileNumber != message.MobileNumber || person.ContactInfo.FaxNumber != message.FaxNumber || person.ContactInfo.WebsiteAddress != message.WebsiteAddress || person.ContactInfo.EmailAddress != message.EmailAddress || person.ContactInfo.InstantMessaging != message.InstantMessaging)
            {
                person.SetContactInfo(message.PhoneNumber, message.MobileNumber, message.FaxNumber, message.WebsiteAddress, message.EmailAddress, message.InstantMessaging, message.UserId);
                await _repository.SaveAsync(person);
            }
        }

        public async Task Handle(ChangePersonShippingAddressCommand message)
        {
            var person = _repository.GetById<Person>(message.PersonId);
            if (person.ShippingAddress == null || person.ShippingAddress.IsDifferentAddress(message.Address, message.City, message.PostalCode, message.Province, message.Country))
            {
                var effectiveDateTime = message.EffectiveDate;
                var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);
                person.ChangeShippingAddress(message.Address, message.City, message.PostalCode, message.Province, !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry(), effectiveDate, message.UserId);
                await _repository.SaveAsync(person);
            }
        }

        public async Task Handle(ChangePersonBillingAddressCommand message)
        {
            var person = _repository.GetById<Person>(message.PersonId);
            if (person.BillingAddress == null || person.BillingAddress.IsDifferentAddress(message.Address, message.City, message.PostalCode, message.Province, message.Country))
            {
                var effectiveDateTime = message.EffectiveDate;
                var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);
                person.ChangeBillingAddress(message.Address, message.City, message.PostalCode, message.Province, !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry(), effectiveDate.Date, message.UserId);
                await _repository.SaveAsync(person);
            }
        }

        public async Task Handle(UnlistPersonCommand message)
        {
            var person = _repository.GetById<Person>(message.PersonId);
            person.Unlist(message.UnlistDate, message.UserId);

            await _repository.SaveAsync(person);
        }

        public class PersonSagaData : SagaData
        {
            public Guid PersonId { get; set; }
        }
    }
}

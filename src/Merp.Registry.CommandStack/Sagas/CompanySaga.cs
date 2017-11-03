using MementoFX.Persistence;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.CommandStack.Model;
using Merp.Registry.CommandStack.Services;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Sagas
{
    public class CompanySaga : Saga<CompanySaga.CompanySagaData>,
        IAmInitiatedBy<RegisterCompanyCommand>,
        IAmInitiatedBy<ImportCompanyCommand>,
        IHandleMessages<ChangeCompanyNameCommand>,
        IHandleMessages<ChangeCompanyLegalAddressCommand>,
        IHandleMessages<ChangeCompanyShippingAddressCommand>,
        IHandleMessages<ChangeCompanyBillingAddressCommand>,
        IHandleMessages<AssociateAdministrativeContactToCompanyCommand>,
        IHandleMessages<AssociateMainContactToCompanyCommand>,
        IHandleMessages<ChangeCompanyContactInfoCommand>
    {
        private readonly IRepository _repository;
        private readonly IBus _bus;
        private readonly IDefaultCountryResolver _defaultCountryResolver;

        public CompanySaga(IRepository repository, IBus bus, IDefaultCountryResolver defaultCountryResolver)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _defaultCountryResolver = defaultCountryResolver ?? throw new ArgumentNullException(nameof(defaultCountryResolver));
        }

        protected override void CorrelateMessages(ICorrelationConfig<CompanySagaData> config)
        {
            config.Correlate<RegisterCompanyCommand>(
                message => message.CompanyId,
                sagaData => sagaData.CompanyId);

            config.Correlate<ImportCompanyCommand>(
                message => message.CompanyId,
                sagaData => sagaData.CompanyId);

            config.Correlate<ChangeCompanyNameCommand>(
                message => message.CompanyId,
                sagaData => sagaData.CompanyId);

            config.Correlate<ChangeCompanyLegalAddressCommand>(
                message => message.CompanyId,
                sagaData => sagaData.CompanyId);

            config.Correlate<ChangeCompanyShippingAddressCommand>(
                message => message.CompanyId,
                sagaData => sagaData.CompanyId);

            config.Correlate<ChangeCompanyBillingAddressCommand>(
                message => message.CompanyId,
                sagaData => sagaData.CompanyId);

            config.Correlate<AssociateAdministrativeContactToCompanyCommand>(
                message => message.CompanyId,
                sagaData => sagaData.CompanyId);

            config.Correlate<AssociateMainContactToCompanyCommand>(
                message => message.CompanyId,
                sagaData => sagaData.CompanyId);

            config.Correlate<ChangeCompanyContactInfoCommand>(
                message => message.CompanyId,
                sagaData => sagaData.CompanyId);
        }

        public Task Handle(RegisterCompanyCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var legalAddressIsDefined = !string.IsNullOrWhiteSpace(message.LegalAddressAddress);
                var legalAddressAddress = legalAddressIsDefined ? message.LegalAddressAddress : null;
                var legalAddressCity = legalAddressIsDefined ? message.LegalAddressCity : null;
                var legalAddressPostalCode = legalAddressIsDefined ? message.LegalAddressPostalCode : null;
                var legalAddressProvince = legalAddressIsDefined ? message.LegalAddressProvince : null;
                var legalAddressCountry = legalAddressIsDefined ? !string.IsNullOrWhiteSpace(message.LegalAddressCountry) ? message.LegalAddressCountry : _defaultCountryResolver.GetDefaultCountry() : null;

                var company = Company.Factory.CreateNewEntry(message.CompanyName, message.VatNumber, message.NationalIdentificationNumber,
                    legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince, legalAddressCountry,
                    message.BillingAddressAddress, message.BillingAddressCity, message.BillingAddressPostalCode, message.BillingAddressProvince, message.BillingAddressCountry,
                    message.ShippingAddressAddress, message.ShippingAddressCity, message.ShippingAddressPostalCode, message.ShippingAddressProvince, message.ShippingAddressCountry);

                if (message.MainContactId.HasValue)
                {
                    Thread.Sleep(10);
                    company.AssociateMainContact(message.MainContactId.Value);
                }
                if (message.AdministrativeContactId.HasValue)
                {
                    Thread.Sleep(10);
                    company.AssociateAdministrativeContact(message.AdministrativeContactId.Value);
                }
                if (!string.IsNullOrWhiteSpace(message.PhoneNumber) || !string.IsNullOrWhiteSpace(message.FaxNumber) || !string.IsNullOrWhiteSpace(message.WebsiteAddress) || !string.IsNullOrWhiteSpace(message.EmailAddress))
                {
                    Thread.Sleep(10);
                    company.SetContactInfo(message.PhoneNumber, null, message.FaxNumber, message.WebsiteAddress, message.EmailAddress, null, null);
                }
                _repository.Save(company);
                this.Data.CompanyId = company.Id;
            });
        }

        public Task Handle(ImportCompanyCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var legalAddressIsDefined = !string.IsNullOrWhiteSpace(message.LegalAddressAddress);
                var legalAddressAddress = legalAddressIsDefined ? message.LegalAddressAddress : null;
                var legalAddressCity = legalAddressIsDefined ? message.LegalAddressCity : null;
                var legalAddressPostalCode = legalAddressIsDefined ? message.LegalAddressPostalCode : null;
                var legalAddressProvince = legalAddressIsDefined ? message.LegalAddressProvince : null;
                var legalAddressCountry = legalAddressIsDefined ? !string.IsNullOrWhiteSpace(message.LegalAddressCountry) ? message.LegalAddressCountry : _defaultCountryResolver.GetDefaultCountry() : null;

                var company = Company.Factory.CreateNewEntryByImport(message.CompanyId, message.RegistrationDate, message.CompanyName, message.VatNumber, message.NationalIdentificationNumber,
                    legalAddressAddress, legalAddressCity, legalAddressPostalCode, legalAddressProvince, legalAddressCountry,
                    message.BillingAddressAddress, message.BillingAddressCity, message.BillingAddressPostalCode, message.BillingAddressProvince, message.BillingAddressCountry,
                    message.ShippingAddressAddress, message.ShippingAddressCity, message.ShippingAddressPostalCode, message.ShippingAddressProvince, message.ShippingAddressCountry);

                if (message.MainContactId.HasValue)
                {
                    Thread.Sleep(10);
                    company.AssociateMainContact(message.MainContactId.Value);
                }
                if (message.AdministrativeContactId.HasValue)
                {
                    Thread.Sleep(10);
                    company.AssociateAdministrativeContact(message.AdministrativeContactId.Value);
                }
                if (!string.IsNullOrWhiteSpace(message.PhoneNumber) || !string.IsNullOrWhiteSpace(message.FaxNumber) || !string.IsNullOrWhiteSpace(message.WebsiteAddress) || !string.IsNullOrWhiteSpace(message.EmailAddress))
                {
                    Thread.Sleep(10);
                    company.SetContactInfo(message.PhoneNumber, null, message.FaxNumber, message.WebsiteAddress, message.EmailAddress, null, null);
                }
                _repository.Save(company);
                this.Data.CompanyId = company.Id;
            });
        }

        public Task Handle(ChangeCompanyNameCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var company = _repository.GetById<Company>(message.CompanyId);
                company.ChangeName(message.CompanyName, message.EffectiveDate);
                _repository.Save(company);
            });
        }

        public Task Handle(ChangeCompanyLegalAddressCommand message)
        {
            return Task.Factory.StartNew(() => {
                var company = _repository.GetById<Company>(message.CompanyId);
                if (company.LegalAddress == null || company.LegalAddress.IsDifferentAddress(message.Address, message.City, message.PostalCode, message.Province, message.Country))
                {
                    var effectiveDateTime = message.EffectiveDate;
                    var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);
                    company.ChangeLegalAddress(message.Address, message.City, message.PostalCode, message.Province, !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry(), effectiveDate);
                    _repository.Save(company);
                }
            });
        }

        public Task Handle(ChangeCompanyShippingAddressCommand message)
        {
            return Task.Factory.StartNew(() => {

                var effectiveDateTime = message.EffectiveDate;
                var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

                var company = _repository.GetById<Company>(message.CompanyId);

                if (effectiveDate > DateTime.Now || company.ShippingAddress == null || company.ShippingAddress.IsDifferentAddress(message.Address, message.City, message.PostalCode, message.Province, message.Country))
                {
                    company.ChangeShippingAddress(message.Address, message.City, message.PostalCode, message.Province, !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry(), effectiveDate);
                    _repository.Save(company);
                }
            });
        }

        public Task Handle(ChangeCompanyBillingAddressCommand message)
        {
            return Task.Factory.StartNew(() => {
                var effectiveDateTime = message.EffectiveDate;
                var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

                var company = _repository.GetById<Company>(message.CompanyId);

                if (effectiveDate > DateTime.Now || company.BillingAddress == null || company.BillingAddress.IsDifferentAddress(message.Address, message.City, message.PostalCode, message.Province, message.Country))
                {
                    company.ChangeBillingAddress(message.Address, message.City, message.PostalCode, message.Province, !string.IsNullOrWhiteSpace(message.Country) ? message.Country : _defaultCountryResolver.GetDefaultCountry(), effectiveDate);
                    _repository.Save(company);
                }
            });
        }

        public Task Handle(AssociateAdministrativeContactToCompanyCommand message)
        {
            return Task.Factory.StartNew(() => {
                var company = _repository.GetById<Company>(message.CompanyId);
                if (message.AdministrativeContactId != company.AdministrativeContactId)
                {
                    company.AssociateAdministrativeContact(message.AdministrativeContactId);
                    _repository.Save(company);
                }
            });
        }

        public Task Handle(AssociateMainContactToCompanyCommand message)
        {
            return Task.Factory.StartNew(() => {
                var company = _repository.GetById<Company>(message.CompanyId);
                if (message.MainContactId != company.MainContactId)
                {
                    company.AssociateMainContact(message.MainContactId);
                    _repository.Save(company);
                }
            });
        }

        public Task Handle(ChangeCompanyContactInfoCommand message)
        {
            return Task.Factory.StartNew(() => {
                var company = _repository.GetById<Company>(message.CompanyId);
                if (company.ContactInfo == null || message.PhoneNumber != company.ContactInfo.PhoneNumber || message.FaxNumber != company.ContactInfo.FaxNumber || message.WebsiteAddress != company.ContactInfo.WebsiteAddress || message.EmailAddress != company.ContactInfo.EmailAddress)
                {
                    company.SetContactInfo(message.PhoneNumber, null, message.FaxNumber, message.WebsiteAddress, message.EmailAddress, null, null);
                    _repository.Save(company);
                }
            });
        }

        public class CompanySagaData : SagaData
        {
            public Guid CompanyId { get; set; }
        }
    }
}

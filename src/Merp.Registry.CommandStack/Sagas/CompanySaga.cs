using Memento.Persistence;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.CommandStack.Model;
using Merp.Registry.CommandStack.Services;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using System;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Sagas
{
    public class CompanySaga : Saga<CompanySaga.CompanySagaData>,
        IAmInitiatedBy<RegisterCompanyCommand>,
        IAmInitiatedBy<ImportCompanyCommand>,
        IHandleMessages<ChangeCompanyNameCommand>
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
        }

        public Task Handle(RegisterCompanyCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var company = Company.Factory.CreateNewEntry(message.CompanyName, message.VatNumber, message.NationalIdentificationNumber);
                company.SetLegalAddress(message.LegalAddressAddress, message.LegalAddressCity, message.LegalAddressPostalCode, message.LegalAddressProvince, !string.IsNullOrWhiteSpace(message.LegalAddressCountry) ? message.LegalAddressCountry : _defaultCountryResolver.GetDefaultCountry());                
                company.SetShippingAddress(message.ShippingAddressAddress, message.ShippingAddressCity, message.ShippingAddressPostalCode, message.ShippingAddressProvince, !string.IsNullOrWhiteSpace(message.ShippingAddressCountry) ? message.ShippingAddressCountry : _defaultCountryResolver.GetDefaultCountry());
                company.SetBillingAddress(message.BillingAddressAddress, message.BillingAddressCity, message.BillingAddressPostalCode, message.BillingAddressProvince, !string.IsNullOrWhiteSpace(message.BillingAddressCountry) ? message.BillingAddressCountry : _defaultCountryResolver.GetDefaultCountry());            
                _repository.Save(company);
                this.Data.CompanyId = company.Id;
            });
        }

        public Task Handle(ImportCompanyCommand message)
        {
            return Task.Factory.StartNew(() =>
            {
                var company = Company.Factory.CreateNewEntryByImport(message.CompanyId, message.CompanyName, message.VatNumber, message.NationalIdentificationNumber);
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

        public class CompanySagaData : SagaData
        {
            public Guid CompanyId { get; set; }
        }
    }
}

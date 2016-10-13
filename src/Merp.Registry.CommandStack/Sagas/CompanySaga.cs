using Memento.Persistence;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.CommandStack.Model;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using System;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Sagas
{
    public class CompanySaga : Saga<CompanySaga.CompanySagaData>,
        IAmInitiatedBy<RegisterCompanyCommand>,
        IHandleMessages<ChangeCompanyNameCommand>
    {
        private readonly IRepository _repository;
        private readonly IBus _bus;

        public CompanySaga(IRepository repository, IBus bus)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));

            this._repository = repository;
            this._bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<CompanySagaData> config)
        {
            config.Correlate<RegisterCompanyCommand>(
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
                var company = Company.Factory.CreateNewEntry(message.CompanyName, message.VatIndex);
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

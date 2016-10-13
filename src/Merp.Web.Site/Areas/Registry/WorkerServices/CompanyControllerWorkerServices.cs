using Merp.Registry.CommandStack.Commands;
using Merp.Registry.QueryStack;
using Merp.Web.Site.Areas.Registry.Models.Company;
using System;
using Memento.Persistence;
using Merp.Registry.CommandStack.Model;
using Rebus.Bus;

namespace Merp.Web.Site.Areas.Registry.WorkerServices
{
    public class CompanyControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; set; }
        public IRepository Repository { get; private set; }

        public CompanyControllerWorkerServices(IBus bus, IDatabase database, IRepository repository)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            this.Bus = bus;
            this.Database = database;
            this.Repository = repository;
        }

        public void AddEntry(AddEntryViewModel model)
        {
            var command = new RegisterCompanyCommand(model.CompanyName, model.VatIndex);
            Bus.Send(command);
        }

        public InfoViewModel GetInfoViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new InfoViewModel()
            {
                CompanyUid = company.Id,
                CompanyName = company.CompanyName,
                VatIndex = company.VatIndex
            };
            return model;
        }

        public ChangeNameViewModel GetChangeNameViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeNameViewModel()
            {
                CompanyUid = company.Id,
                CurrentCompanyName = company.CompanyName             
            };
            return model;
        }

        public void PostChangeNameViewModel(ChangeNameViewModel model)
        {
            var cmd = new ChangeCompanyNameCommand(model.CompanyUid, model.NewCompanyName, model.EffectiveDate);
            Bus.Send(cmd);
        }
    }
}
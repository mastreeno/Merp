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
            var shippingAddressIsDefined = !string.IsNullOrWhiteSpace(model.ShippingAddress.Address) && !string.IsNullOrWhiteSpace(model.ShippingAddress.City);
            var billingAddressIsDefined = !string.IsNullOrWhiteSpace(model.BillingAddress.Address) && !string.IsNullOrWhiteSpace(model.BillingAddress.City);

            var command = new RegisterCompanyCommand(model.CompanyName, model.NationalIdentificationNumber, model.VatNumber,
                model.LegalAddress.Address,
                model.LegalAddress.PostalCode,
                model.LegalAddress.City,
                model.LegalAddress.Province,
                model.LegalAddress.Country,

                shippingAddressIsDefined ? model.ShippingAddress.Address : model.LegalAddress.Address,
                shippingAddressIsDefined ? model.ShippingAddress.PostalCode : model.LegalAddress.PostalCode,
                shippingAddressIsDefined ? model.ShippingAddress.City : model.LegalAddress.City,
                shippingAddressIsDefined ? model.ShippingAddress.Province : model.LegalAddress.Province,
                shippingAddressIsDefined ? model.ShippingAddress.Country : model.LegalAddress.Country,

                billingAddressIsDefined ? model.BillingAddress.Address : model.LegalAddress.Address,
                billingAddressIsDefined ? model.BillingAddress.PostalCode : model.LegalAddress.PostalCode,
                billingAddressIsDefined ? model.BillingAddress.City : model.LegalAddress.City,
                billingAddressIsDefined ? model.BillingAddress.Province : model.LegalAddress.Province,
                billingAddressIsDefined ? model.BillingAddress.Country : model.LegalAddress.Country
                );
            Bus.Send(command);
        }

        public InfoViewModel GetInfoViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new InfoViewModel()
            {
                CompanyUid = company.Id,
                CompanyName = company.CompanyName,
                VatNumber = company.VatIndex,
                LegalAddress = new Models.PostalAddress
                {
                    Address = company.LegalAddress.Address,
                    City = company.LegalAddress.Address,
                    Country = company.LegalAddress.Country,
                    PostalCode = company.LegalAddress.PostalCode,
                    Province = company.LegalAddress.Province
                }
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
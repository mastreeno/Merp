using Memento.Persistence;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.CommandStack.Model;
using Merp.Registry.QueryStack;
using Merp.Web.Site.Areas.Registry.Models;
using Merp.Web.Site.Areas.Registry.Models.Company;
using Rebus.Bus;
using System;
using System.Linq;

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
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var shippingAddressIsDefined = !string.IsNullOrWhiteSpace(model.ShippingAddress.Address) && !string.IsNullOrWhiteSpace(model.ShippingAddress.City);
            var billingAddressIsDefined = !string.IsNullOrWhiteSpace(model.BillingAddress.Address) && !string.IsNullOrWhiteSpace(model.BillingAddress.City);

            string companyName = model.CompanyName;
            string nationalIdentificationNumber = model.NationalIdentificationNumber;
            string vatNumber = model.VatNumber;

            string legalAddressAddress = model.LegalAddress.Address;
            string legalAddressPostalCode = model.LegalAddress.PostalCode;
            string legalAddressCity = model.LegalAddress.City;
            string legalAddressProvince = model.LegalAddress.Province;
            string legalAddressCountry = model.LegalAddress.Country;

            string shippingAddressAddress = shippingAddressIsDefined ? model.ShippingAddress.Address : model.LegalAddress.Address;
            string shippingAddressPostalCode = shippingAddressIsDefined ? model.ShippingAddress.PostalCode : model.LegalAddress.PostalCode;
            string shippingAddressCity = shippingAddressIsDefined ? model.ShippingAddress.City : model.LegalAddress.City;
            string shippingAddressProvince = shippingAddressIsDefined ? model.ShippingAddress.Province : model.LegalAddress.Province;
            string shippingAddressCountry = shippingAddressIsDefined ? model.ShippingAddress.Country : model.LegalAddress.Country;

            string billingAddressAddress = billingAddressIsDefined ? model.BillingAddress.Address : model.LegalAddress.Address;
            string billingAddressPostalCode = billingAddressIsDefined ? model.BillingAddress.PostalCode : model.LegalAddress.PostalCode;
            string billingAddressCity = billingAddressIsDefined ? model.BillingAddress.City : model.LegalAddress.City;
            string billingAddressProvince = billingAddressIsDefined ? model.BillingAddress.Province : model.LegalAddress.Province;
            string billingAddressCountry = billingAddressIsDefined ? model.BillingAddress.Country : model.LegalAddress.Country;

            string phoneNumber = model.PhoneNumber;
            string faxNumber = model.FaxNumber;
            string websiteAddress = model.WebsiteAddress;
            string emailAddress = model.EmailAddress;

            Guid? mainContactId = model.MainContact == null ? default(Guid?) : model.MainContact.OriginalId;
            Guid? administrativeContactId = model.AdministrativeContact == null ? default(Guid?) : model.AdministrativeContact.OriginalId;

            var command = new RegisterCompanyCommand(
                companyName, 
                nationalIdentificationNumber, 
                vatNumber,

                legalAddressAddress,
                legalAddressPostalCode,
                legalAddressCity,
                legalAddressProvince,
                legalAddressCountry,

                shippingAddressAddress,
                shippingAddressPostalCode,
                shippingAddressCity,
                shippingAddressProvince,
                shippingAddressCountry,

                billingAddressAddress,
                billingAddressPostalCode,
                billingAddressCity,
                billingAddressProvince,
                billingAddressCountry,

                mainContactId,
                administrativeContactId,
                phoneNumber,
                faxNumber,
                websiteAddress,
                emailAddress
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
                VatNumber = company.VatNumber,
                NationalIdentificationNumber = company.NationalIdentificationNumber,
                LegalAddress = new Models.PostalAddress
                {
                    Address = company.LegalAddress.Address,
                    City = company.LegalAddress.City,
                    Country = company.LegalAddress.Country,
                    PostalCode = company.LegalAddress.PostalCode,
                    Province = company.LegalAddress.Province
                },
                ShippingAddress = new Models.PostalAddress
                {
                    Address = company.ShippingAddress.Address,
                    City = company.ShippingAddress.City,
                    Country = company.ShippingAddress.Country,
                    PostalCode = company.ShippingAddress.PostalCode,
                    Province = company.ShippingAddress.Province
                },
                BillingAddress = new Models.PostalAddress
                {
                    Address = company.BillingAddress.Address,
                    City = company.BillingAddress.City,
                    Country = company.BillingAddress.Country,
                    PostalCode = company.BillingAddress.PostalCode,
                    Province = company.BillingAddress.Province
                }
            };
            if (company.ContactInfo != null)
            {
                model.PhoneNumber = company.ContactInfo.PhoneNumber;
                model.FaxNumber = company.ContactInfo.FaxNumber;
                model.WebsiteAddress = company.ContactInfo.WebsiteAddress;
                model.EmailAddress = company.ContactInfo.EmailAddress;
            }
            if (company.MainContactId.HasValue)
            {
                model.MainContactName = Database.Parties.OfType<Merp.Registry.QueryStack.Model.Person>()
                                                  .Where(p => p.OriginalId == company.MainContactId.Value)
                                                  .Select(p => p.DisplayName)
                                                  .Single();
            }
            if (company.AdministrativeContactId.HasValue)
            {
                model.AdministrativeContactName = Database.Parties.OfType<Merp.Registry.QueryStack.Model.Person>()
                                                 .Where(p => p.OriginalId == company.AdministrativeContactId.Value)
                                                 .Select(p => p.DisplayName)
                                                 .Single();
            }
            return model;
        }

        public ChangeNameViewModel GetChangeNameViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeNameViewModel()
            {
                CompanyId = company.Id,
                CurrentCompanyName = company.CompanyName             
            };
            return model;
        }

        public void ChangeName(ChangeNameViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new ChangeCompanyNameCommand(model.CompanyId, model.NewCompanyName, model.EffectiveDate);
            Bus.Send(cmd);
        }

        public ChangeLegalAddressViewModel GetChangeLegalAddressViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeLegalAddressViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                LegalAddress = new Models.PostalAddress
                {
                    Address = company.LegalAddress.Address,
                    City = company.LegalAddress.City,
                    PostalCode = company.LegalAddress.PostalCode,
                    Province = company.LegalAddress.Province,
                    Country = company.LegalAddress.Country
                }
            };
            return model;
        }

        public void ChangeLegalAddress(ChangeLegalAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new ChangeCompanyLegalAddressCommand(model.CompanyId,
                model.LegalAddress.Address,
                model.LegalAddress.PostalCode,
                model.LegalAddress.City,
                model.LegalAddress.Province,
                model.LegalAddress.Country);

            Bus.Send(cmd);
        }
        
        public ChangeShippingAddressViewModel GetChangeShippingAddressViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeShippingAddressViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                ShippingAddress = new Models.PostalAddress
                {
                    Address = company.ShippingAddress.Address,
                    City = company.ShippingAddress.City,
                    PostalCode = company.ShippingAddress.PostalCode,
                    Province = company.ShippingAddress.Province,
                    Country = company.ShippingAddress.Country
                }
            };
            return model;
        }

        public void ChangeShippingAddress(ChangeShippingAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new ChangeCompanyShippingAddressCommand(model.CompanyId,
                model.ShippingAddress.Address,
                model.ShippingAddress.PostalCode,
                model.ShippingAddress.City,
                model.ShippingAddress.Province,
                model.ShippingAddress.Country);

            Bus.Send(cmd);
        }

        public ChangeBillingAddressViewModel GetChangeBillingAddressViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeBillingAddressViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                BillingAddress = new Models.PostalAddress
                {
                    Address = company.BillingAddress.Address,
                    City = company.BillingAddress.City,
                    PostalCode = company.BillingAddress.PostalCode,
                    Province = company.BillingAddress.Province,
                    Country = company.BillingAddress.Country
                }
            };
            return model;
        }

        public void ChangeBillingAddress(ChangeBillingAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new ChangeCompanyBillingAddressCommand(model.CompanyId,
                model.BillingAddress.Address,
                model.BillingAddress.PostalCode,
                model.BillingAddress.City,
                model.BillingAddress.Province,
                model.BillingAddress.Country);

            Bus.Send(cmd);
        }

        public AssociateAdministrativeContactViewModel GetAssociateAdministrativeContactViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new AssociateAdministrativeContactViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName
            };
            if (company.AdministrativeContactId.HasValue)
            {
                model.AdministrativeContact = Database.Parties.OfType<Merp.Registry.QueryStack.Model.Person>()
                                                .Where(p => p.OriginalId == company.AdministrativeContactId.Value) 
                                                .Select(p => new PersonInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName })
                                                .Single();
            }
            
            return model;
        }

        public void AssociateAdministrativeContact(AssociateAdministrativeContactViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new AssociateAdministrativeContactToCompanyCommand(model.CompanyId, model.AdministrativeContact.OriginalId);

            Bus.Send(cmd);
        }



        public AssociateMainContactViewModel GetAssociateMainContactViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new AssociateMainContactViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName
            };
            if (company.MainContactId.HasValue)
            {
                model.MainContact = Database.Parties.OfType<Merp.Registry.QueryStack.Model.Person>()
                                                .Where(p => p.OriginalId == company.MainContactId.Value)
                                                .Select(p => new PersonInfo { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName })
                                                .Single();
            }

            return model;
        }

        public void AssociateMainContact(AssociateMainContactViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new AssociateMainContactToCompanyCommand(model.CompanyId, model.MainContact.OriginalId);

            Bus.Send(cmd);
        }

        public ChangeContactInfoViewModel GetChangeContactInfoViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeContactInfoViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName
            };
            if(company.ContactInfo != null)
            {
                model.PhoneNumber = company.ContactInfo.PhoneNumber;
                model.FaxNumber = company.ContactInfo.FaxNumber;
                model.WebsiteAddress = company.ContactInfo.WebsiteAddress;
                model.EmailAddress = company.ContactInfo.EmailAddress;
            }
            return model;
        }

        public void ChangeContactInfo(ChangeContactInfoViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new ChangeCompanyContactInfoCommand(model.CompanyId, model.PhoneNumber, model.FaxNumber, model.WebsiteAddress, model.EmailAddress);

            Bus.Send(cmd);
        }
    }
}
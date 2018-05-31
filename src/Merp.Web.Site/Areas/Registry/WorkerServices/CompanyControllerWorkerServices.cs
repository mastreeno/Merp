using MementoFX.Persistence;
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
            this.Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public AddEntryViewModel GetAddEntryViewModel()
        {
            return new AddEntryViewModel();
        }

        public void AddEntry(AddEntryViewModel model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));

            var companyName = model.CompanyName;
            var nationalIdentificationNumber = model.NationalIdentificationNumber;
            var vatNumber = model.VatNumber;

            var legalAddressAddress = model.LegalAddress.Address;
            var legalAddressPostalCode = model.LegalAddress.PostalCode;
            var legalAddressCity = model.LegalAddress.City;
            var legalAddressProvince = model.LegalAddress.Province;
            var legalAddressCountry = model.LegalAddress.Country;
            
            var shippingAddressAddress = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.Address : model.ShippingAddress.Address;
            var shippingAddressPostalCode = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.PostalCode : model.ShippingAddress.PostalCode;
            var shippingAddressCity = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.City: model.ShippingAddress.City;
            var shippingAddressProvince = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.Province : model.ShippingAddress.Province;
            var shippingAddressCountry = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.Country : model.ShippingAddress.Country;

            var billingAddressAddress = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.Address : model.BillingAddress.Address;
            var billingAddressPostalCode = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.PostalCode : model.BillingAddress.PostalCode;
            var billingAddressCity = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.City : model.BillingAddress.City;
            var billingAddressProvince = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.Province : model.BillingAddress.Province;
            var billingAddressCountry = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.Country : model.BillingAddress.Country;

            var phoneNumber = model.PhoneNumber;
            var faxNumber = model.FaxNumber;
            var websiteAddress = model.WebsiteAddress;
            var emailAddress = model.EmailAddress;

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
                emailAddress,

                model.Skype
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
                LegalAddress = new Models.PostalAddress(),
                ShippingAddress = new Models.PostalAddress(),
                BillingAddress = new Models.PostalAddress()
            };
            if (company.LegalAddress != null)
            {
                model.LegalAddress.Address = company.LegalAddress.Address;
                model.LegalAddress.City = company.LegalAddress.City;
                model.LegalAddress.Country = company.LegalAddress.Country;
                model.LegalAddress.PostalCode = company.LegalAddress.PostalCode;
                model.LegalAddress.Province = company.LegalAddress.Province;
            }
            if (company.ShippingAddress != null)
            {
                model.ShippingAddress.Address = company.ShippingAddress.Address;
                model.ShippingAddress.City = company.ShippingAddress.City;
                model.ShippingAddress.Country = company.ShippingAddress.Country;
                model.ShippingAddress.PostalCode = company.ShippingAddress.PostalCode;
                model.ShippingAddress.Province = company.ShippingAddress.Province;
            }
            if (company.BillingAddress != null)
            {
                model.BillingAddress.Address = company.BillingAddress.Address;
                model.BillingAddress.City = company.BillingAddress.City;
                model.BillingAddress.Country = company.BillingAddress.Country;
                model.BillingAddress.PostalCode = company.BillingAddress.PostalCode;
                model.BillingAddress.Province = company.BillingAddress.Province;
            }
            if (company.ContactInfo != null)
            {
                model.PhoneNumber = company.ContactInfo.PhoneNumber;
                model.FaxNumber = company.ContactInfo.FaxNumber;
                model.WebsiteAddress = company.ContactInfo.WebsiteAddress;
                model.EmailAddress = company.ContactInfo.EmailAddress;
            }
            if (company.MainContactId.HasValue)
            {
                model.MainContactName = Database.People
                                                  .Where(p => p.OriginalId == company.MainContactId.Value)
                                                  .Select(p => p.DisplayName)
                                                  .Single();
            }
            if (company.AdministrativeContactId.HasValue)
            {
                model.AdministrativeContactName = Database.People
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

        public ChangeNameViewModel GetChangeNameViewModel(ChangeNameViewModel model)
        {
            var company = Repository.GetById<Company>(model.CompanyId);
            var viewModel = new ChangeNameViewModel()
            {
                CompanyId = company.Id,
                CurrentCompanyName = company.CompanyName,
                EffectiveDate = model.EffectiveDate,
                NewCompanyName = model.NewCompanyName
            };
            return viewModel;
        }

        public ChangeNameViewModel.CompanyDto GetChangeNameViewModelCompanyDto(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeNameViewModel.CompanyDto
            {
                RegistrationDate = company.RegistrationDate
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
                LegalAddress = new Models.PostalAddress()
            };
            if (company.LegalAddress != null)
            {
                model.LegalAddress.Address = company.LegalAddress.Address;
                model.LegalAddress.City = company.LegalAddress.City;
                model.LegalAddress.Country = company.LegalAddress.Country;
                model.LegalAddress.PostalCode = company.LegalAddress.PostalCode;
                model.LegalAddress.Province = company.LegalAddress.Province;
            }
            return model;
        }

        public ChangeLegalAddressViewModel GetChangeLegalAddressViewModel(ChangeLegalAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var company = Repository.GetById<Company>(model.CompanyId);

            var viewModel = new ChangeLegalAddressViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                LegalAddress = new Models.PostalAddress
                {
                    Address = model.LegalAddress.Address,
                    City = model.LegalAddress.City,
                    PostalCode = model.LegalAddress.PostalCode,
                    Province = model.LegalAddress.Province,
                    Country = model.LegalAddress.Country
                }
            };
            return viewModel;
        }

        public ChangeLegalAddressViewModel.CompanyDto GetChangeLegalAddressViewModelCompanyDto(Guid companyId)
        {            
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeLegalAddressViewModel.CompanyDto
            {
                RegistrationDate = company.RegistrationDate
            };
            return model;
        }

        public void ChangeLegalAddress(ChangeLegalAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangeCompanyLegalAddressCommand(model.CompanyId,
                model.LegalAddress.Address,
                model.LegalAddress.PostalCode,
                model.LegalAddress.City,
                model.LegalAddress.Province,
                model.LegalAddress.Country,
                effectiveDate);

            Bus.Send(cmd);
        }
        
        public ChangeShippingAddressViewModel GetChangeShippingAddressViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeShippingAddressViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                ShippingAddress = new Models.PostalAddress()
            };
            if (company.ShippingAddress != null)
            {
                model.ShippingAddress.Address = company.ShippingAddress.Address;
                model.ShippingAddress.City = company.ShippingAddress.City;
                model.ShippingAddress.Country = company.ShippingAddress.Country;
                model.ShippingAddress.PostalCode = company.ShippingAddress.PostalCode;
                model.ShippingAddress.Province = company.ShippingAddress.Province;
            }
            return model;
        }

        public ChangeShippingAddressViewModel GetChangeShippingAddressViewModel(ChangeShippingAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var company = Repository.GetById<Company>(model.CompanyId);

            var viewModel = new ChangeShippingAddressViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                ShippingAddress = new Models.PostalAddress
                {
                    Address = model.ShippingAddress.Address,
                    City = model.ShippingAddress.City,
                    PostalCode = model.ShippingAddress.PostalCode,
                    Province = model.ShippingAddress.Province,
                    Country = model.ShippingAddress.Country
                }
            };
            return viewModel;
        }

        public ChangeShippingAddressViewModel.CompanyDto GetChangeShippingAddressViewModelCompanyDto(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeShippingAddressViewModel.CompanyDto
            {
                RegistrationDate = company.RegistrationDate
            };
            return model;
        }

        public void ChangeShippingAddress(ChangeShippingAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangeCompanyShippingAddressCommand(model.CompanyId,
                model.ShippingAddress.Address,
                model.ShippingAddress.PostalCode,
                model.ShippingAddress.City,
                model.ShippingAddress.Province,
                model.ShippingAddress.Country,
                effectiveDate);

            Bus.Send(cmd);
        }

        public ChangeBillingAddressViewModel GetChangeBillingAddressViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeBillingAddressViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                BillingAddress = new Models.PostalAddress()
            };
            if (company.BillingAddress != null)
            {
                model.BillingAddress.Address = company.BillingAddress.Address;
                model.BillingAddress.City = company.BillingAddress.City;
                model.BillingAddress.Country = company.BillingAddress.Country;
                model.BillingAddress.PostalCode = company.BillingAddress.PostalCode;
                model.BillingAddress.Province = company.BillingAddress.Province;
            }
            return model;
        }

        public ChangeBillingAddressViewModel GetChangeBillingAddressViewModel(ChangeBillingAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var company = Repository.GetById<Company>(model.CompanyId);

            var viewModel = new ChangeBillingAddressViewModel()
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                BillingAddress = new Models.PostalAddress
                {
                    Address = model.BillingAddress.Address,
                    City = model.BillingAddress.City,
                    PostalCode = model.BillingAddress.PostalCode,
                    Province = model.BillingAddress.Province,
                    Country = model.BillingAddress.Country
                }
            };
            return viewModel;
        }

        public ChangeBillingAddressViewModel.CompanyDto GetChangeBillingAddressViewModelCompanyDto(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new ChangeBillingAddressViewModel.CompanyDto
            {
                RegistrationDate = company.RegistrationDate
            };
            return model;
        }

        public void ChangeBillingAddress(ChangeBillingAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangeCompanyBillingAddressCommand(model.CompanyId,
                model.BillingAddress.Address,
                model.BillingAddress.PostalCode,
                model.BillingAddress.City,
                model.BillingAddress.Province,
                model.BillingAddress.Country,
                effectiveDate);

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
                model.AdministrativeContact = Database.People
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
                model.MainContact = Database.People
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
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.QueryStack;
using Merp.Web.Site.Areas.Registry.Models.Person;
using System;
using System.Linq;
using MementoFX.Persistence;
using Merp.Registry.CommandStack.Model;
using Rebus.Bus;

namespace Merp.Web.Site.Areas.Registry.WorkerServices
{
    public class PersonControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; set; }
        public IRepository Repository { get; private set; }

        public PersonControllerWorkerServices(IBus bus, IDatabase database, IRepository repository)
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
            var nationalIdentificationNumber = string.IsNullOrWhiteSpace(model.NationalIdentificationNumber) ? default(string) : model.NationalIdentificationNumber.Trim().ToUpper();

            var shippingAddressAddress = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.Address : model.ShippingAddress.Address;
            var shippingAddressPostalCode = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.PostalCode : model.ShippingAddress.PostalCode;
            var shippingAddressCity = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.City : model.ShippingAddress.City;
            var shippingAddressProvince = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.Province : model.ShippingAddress.Province;
            var shippingAddressCountry = model.AcquireShippingAddressFromLegalAddress ? model.LegalAddress.Country : model.ShippingAddress.Country;

            var billingAddressAddress = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.Address : model.BillingAddress.Address;
            var billingAddressPostalCode = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.PostalCode : model.BillingAddress.PostalCode;
            var billingAddressCity = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.City : model.BillingAddress.City;
            var billingAddressProvince = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.Province : model.BillingAddress.Province;
            var billingAddressCountry = model.AcquireBillingAddressFromLegalAddress ? model.LegalAddress.Country : model.BillingAddress.Country;

            var command = new RegisterPersonCommand(
                model.FirstName, 
                model.LastName,
                nationalIdentificationNumber, 
                model.VatNumber, 
                model.LegalAddress.Address, 
                model.LegalAddress.City, 
                model.LegalAddress.PostalCode, 
                model.LegalAddress.Province, 
                model.LegalAddress.Country,
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
                model.PhoneNumber,
                model.MobileNumber,
                model.FaxNumber,
                model.WebsiteAddress,
                model.EmailAddress,
                model.InstantMessaging
                );

            Bus.Send(command);
        }

        public InfoViewModel GetInfoViewModel(Guid personId)
        {
            var person = Repository.GetById<Person>(personId);
            var model = new InfoViewModel()
            {
                OriginalId = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                NationalIdentificationNumber = person.NationalIdentificationNumber,
                VatNumber = person.VatNumber,

            };
            if(person.LegalAddress != null)
            {
                model.LegalAddress = new Models.PostalAddress
                {
                    Address = person.LegalAddress.Address,
                    City = person.LegalAddress.City,
                    Country = person.LegalAddress.Country,
                    PostalCode = person.LegalAddress.PostalCode,
                    Province = person.LegalAddress.Province
                };
            }
            if (person.ShippingAddress != null)
            {
                model.ShippingAddress = new Models.PostalAddress
                {
                    Address = person.ShippingAddress.Address,
                    City = person.ShippingAddress.City,
                    Country = person.ShippingAddress.Country,
                    PostalCode = person.ShippingAddress.PostalCode,
                    Province = person.ShippingAddress.Province
                };
            }
            if (person.BillingAddress != null)
            {
                model.BillingAddress = new Models.PostalAddress
                {
                    Address = person.BillingAddress.Address,
                    City = person.BillingAddress.City,
                    Country = person.BillingAddress.Country,
                    PostalCode = person.BillingAddress.PostalCode,
                    Province = person.BillingAddress.Province
                };
            }
            if (person.ContactInfo != null)
            {
                model.PhoneNumber = person.ContactInfo.PhoneNumber;
                model.MobileNumber = person.ContactInfo.MobileNumber;
                model.FaxNumber = person.ContactInfo.FaxNumber;
                model.WebsiteAddress = person.ContactInfo.WebsiteAddress;
                model.EmailAddress = person.ContactInfo.EmailAddress;
                model.InstantMessaging = person.ContactInfo.InstantMessaging;
            }
            model.Id = Database.People
                                .Where(p => p.OriginalId == person.Id)
                                .Select(p => p.Id)
                                .Single();
            return model;
        }
        
        public ChangeLegalAddressViewModel GetChangeAddressViewModel(Guid personId)
        {
            var person = Repository.GetById<Person>(personId);
            var model = new ChangeLegalAddressViewModel()
            {
                PersonId = person.Id,
                PersonFirstName = person.FirstName,
                PersonLastName = person.LastName
            };
            if(person.LegalAddress != null)
            {
                model.Address = new Models.PostalAddress
                {
                    Address = person.LegalAddress.Address,
                    City = person.LegalAddress.City,
                    PostalCode = person.LegalAddress.PostalCode,
                    Province = person.LegalAddress.Province,
                    Country = person.LegalAddress.Country
                };
            }
            return model;
        }

        public ChangeLegalAddressViewModel GetChangeAddressViewModel(ChangeLegalAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var person = Repository.GetById<Person>(model.PersonId);
            var rehydratedModel = new ChangeLegalAddressViewModel()
            {
                PersonId = person.Id,
                PersonFirstName = person.FirstName,
                PersonLastName = person.LastName,
                EffectiveDate = model.EffectiveDate,
                Address = new Models.PostalAddress
                {
                    Address = model.Address.Address,
                    City = model.Address.City,
                    PostalCode = model.Address.PostalCode,
                    Province = model.Address.Province,
                    Country = model.Address.Country
                }
            };
            return rehydratedModel;
        }

        public ChangeLegalAddressViewModel.PersonDto GetChangeLegalAddressViewModelPersonDto(Guid personId)
        {
            var person = Repository.GetById<Person>(personId);            
            var model = new ChangeLegalAddressViewModel.PersonDto
            {
                RegistrationDate = person.RegistrationDate
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

            var cmd = new ChangePersonLegalAddressCommand(model.PersonId,
                model.Address.Address,
                model.Address.PostalCode,
                model.Address.City,
                model.Address.Province,
                model.Address.Country,
                effectiveDate);

            Bus.Send(cmd);
        }

        public ChangeContactInfoViewModel GetChangeContactInfoViewModel(Guid personId)
        {
            var person = Repository.GetById<Person>(personId);
            var model = new ChangeContactInfoViewModel()
            {
                PersonId = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName
            };
            if (person.ContactInfo != null)
            {
                model.PhoneNumber = person.ContactInfo.PhoneNumber;
                model.MobileNumber = person.ContactInfo.MobileNumber;
                model.FaxNumber = person.ContactInfo.FaxNumber;
                model.WebsiteAddress = person.ContactInfo.WebsiteAddress;
                model.EmailAddress = person.ContactInfo.EmailAddress;
                model.InstantMessaging = person.ContactInfo.InstantMessaging;
            }
            return model;
        }

        public void ChangeContactInfo(ChangeContactInfoViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new ChangePersonContactInfoCommand(model.PersonId, model.PhoneNumber, model.MobileNumber, model.FaxNumber, model.WebsiteAddress, model.EmailAddress, model.InstantMessaging);

            Bus.Send(cmd);
        }

        public ChangeShippingAddressViewModel.PersonDto GetChangeShippingAddressViewModelPersonDto(Guid personId)
        {
            var person = Repository.GetById<Person>(personId);
            var model = new ChangeShippingAddressViewModel.PersonDto
            {
                RegistrationDate = person.RegistrationDate
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

            var cmd = new ChangePersonShippingAddressCommand(
                model.PersonId,
                model.Address.Address,
                model.Address.PostalCode,
                model.Address.City,
                model.Address.Province,
                model.Address.Country,
                effectiveDate);

            Bus.Send(cmd);
        }

        public ChangeBillingAddressViewModel.PersonDto GetChangeBillingAddressViewModelPersonDto(Guid personId)
        {
            var person = Repository.GetById<Person>(personId);
            var model = new ChangeBillingAddressViewModel.PersonDto
            {
                RegistrationDate = person.RegistrationDate
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

            var cmd = new ChangePersonBillingAddressCommand(
                model.PersonId,
                model.Address.Address,
                model.Address.PostalCode,
                model.Address.City,
                model.Address.Province,
                model.Address.Country,
                effectiveDate);

            Bus.Send(cmd);
        }
    }
}
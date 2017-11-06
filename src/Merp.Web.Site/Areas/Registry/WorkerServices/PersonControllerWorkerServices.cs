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
            // con il model ricevuto viene creato il comando
            var command = new RegisterPersonCommand(
                model.FirstName,
                model.LastName,
                nationalIdentificationNumber,
                model.VatNumber,

                model.Address.Address,
                model.Address.City,
                model.Address.PostalCode,
                model.Address.Province,
                model.Address.Country,

                model.PhoneNumber,
                model.MobileNumber,
                model.FaxNumber,
                model.WebsiteAddress,
                model.EmailAddress,
                model.InstantMessaging,
                model.Linkedin
                );

            // il comando viene lanciato da rebus
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
            if (person.LegalAddress != null)
            {
                model.Address = new Models.PostalAddress
                {
                    Address = person.LegalAddress.Address,
                    City = person.LegalAddress.City,
                    Country = person.LegalAddress.Country,
                    PostalCode = person.LegalAddress.PostalCode,
                    Province = person.LegalAddress.Province
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
                model.Linkedin = person.ContactInfo.Linkedin;
            }
            model.Id = Database.People
                                .Where(p => p.OriginalId == person.Id)
                                .Select(p => p.Id)
                                .Single();
            return model;
        }

        public ChangeAddressViewModel GetChangeAddressViewModel(Guid personId)
        {
            var person = Repository.GetById<Person>(personId);
            var model = new ChangeAddressViewModel()
            {
                PersonId = person.Id,
                PersonFirstName = person.FirstName,
                PersonLastName = person.LastName
            };
            if (person.LegalAddress != null)
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

        public ChangeAddressViewModel GetChangeAddressViewModel(ChangeAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var person = Repository.GetById<Person>(model.PersonId);
            var rehydratedModel = new ChangeAddressViewModel()
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

        public ChangeAddressViewModel.PersonDto GetChangeAddressViewModelPersonDto(Guid personId)
        {
            var person = Repository.GetById<Person>(personId);
            var model = new ChangeAddressViewModel.PersonDto
            {
                RegistrationDate = person.RegistrationDate
            };
            return model;
        }

        public void ChangeAddress(ChangeAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangePersonAddressCommand(model.PersonId,
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

            var cmd = new ChangePersonContactInfoCommand(model.PersonId, model.PhoneNumber, model.MobileNumber, model.FaxNumber, model.WebsiteAddress, model.EmailAddress, model.InstantMessaging, model.Linkedin);

            Bus.Send(cmd);
        }
    }
}
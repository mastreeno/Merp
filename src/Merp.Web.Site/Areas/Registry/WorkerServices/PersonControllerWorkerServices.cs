using Merp.Registry.CommandStack.Commands;
using Merp.Registry.QueryStack;
using Merp.Web.Site.Areas.Registry.Models.Person;
using System;
using System.Linq;
using Memento.Persistence;
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
            if(bus==null)
                throw new ArgumentNullException(nameof(bus));
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            this.Bus = bus;
            this.Database = database;
            this.Repository = repository;
        }

        public AddEntryViewModel GetAddEntryViewModel()
        {
            return new AddEntryViewModel();
        }

        public void AddEntry(AddEntryViewModel model)
        {

            var command = new RegisterPersonCommand(
                model.FirstName, 
                model.LastName, 
                model.NationalIdentificationNumber, 
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
                model.Address = new Models.PostalAddress
                {
                    Address = person.LegalAddress.Address,
                    City = person.LegalAddress.City,
                    Country = person.LegalAddress.Country,
                    PostalCode = person.LegalAddress.PostalCode,
                    Province = person.LegalAddress.Province
                };
            }
            if(person.ContactInfo != null)
            {
                model.PhoneNumber = person.ContactInfo.PhoneNumber;
                model.MobileNumber = person.ContactInfo.MobileNumber;
                model.FaxNumber = person.ContactInfo.FaxNumber;
                model.WebsiteAddress = person.ContactInfo.WebsiteAddress;
                model.EmailAddress = person.ContactInfo.EmailAddress;
                model.InstantMessaging = person.ContactInfo.InstantMessaging;
            }
            model.Id = Database.Parties.OfType<Merp.Registry.QueryStack.Model.Person>()
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
                PersonId = person.Id
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

        public void ChangeAddress(ChangeAddressViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new ChangePersonAddressCommand(model.PersonId,
                model.Address.Address,
                model.Address.PostalCode,
                model.Address.City,
                model.Address.Province,
                model.Address.Country);

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
    }
}
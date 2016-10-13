using Merp.Registry.CommandStack.Commands;
using Merp.Registry.QueryStack;
using Merp.Web.Site.Areas.Registry.Models.Person;
using System;
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

        public void AddEntry(AddEntryViewModel model)
        {
            var command = new RegisterPersonCommand(model.FirstName, model.LastName, model.DateOfBirth);
            Bus.Send(command);
        }

        public InfoViewModel GetInfoViewModel(Guid companyId)
        {
            var person = Repository.GetById<Person>(companyId);
            var model = new InfoViewModel()
            {
                PersonUid = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName
            };
            return model;
        }
    }
}
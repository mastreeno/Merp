using Merp.Registry.CommandStack.Commands;
using Merp.Registry.QueryStack;
using Merp.Registry.Web.Api.Internal.Models;
using Merp.Web;
using Microsoft.EntityFrameworkCore;
using Rebus.Bus;

namespace Merp.Registry.Web.Api.Internal
{
    public class PersonApiServices : IPersonApiServices
    {
        public IDatabase Database { get; }
        public IBus Bus { get; }
        public IAppContext AppContext { get; }

        public PersonApiServices(IDatabase database, IBus bus, IAppContext appContext)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            AppContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        }

        public async Task<IEnumerable<PersonInfoModel>> SearchPeopleByPatternAsync(string query)
        {
            var model = await Database.People
                .Where(p => p.DisplayName.StartsWith(query))
                .OrderBy(p => p.DisplayName)
                .Select(p => new PersonInfoModel { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName })
                .ToArrayAsync();

            return model;
        }

        public async Task RegisterPersonAsync(RegisterPersonModel model)
        {
            var userId = AppContext.UserId;
            var nationalIdentificationNumber = string.IsNullOrWhiteSpace(model.NationalIdentificationNumber) ? default : model.NationalIdentificationNumber.Trim().ToUpper();

            var command = new RegisterPersonCommand(
                userId,
                model.FirstName,
                model.LastName,
                nationalIdentificationNumber,
                model.VatNumber,
                model.Address.Address,
                model.Address.City,
                model.Address.PostalCode,
                model.Address.Province,
                model.Address.Country,
                model.Address.Address,
                model.Address.City,
                model.Address.PostalCode,
                model.Address.Province,
                model.Address.Country,
                model.Address.Address,
                model.Address.City,
                model.Address.PostalCode,
                model.Address.Province,
                model.Address.Country,
                null, null, null, null, null, null);

            await Bus.Send(command);
        }
    }
}

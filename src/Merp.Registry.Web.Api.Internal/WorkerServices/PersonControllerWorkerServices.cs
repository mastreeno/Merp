using Merp.Registry.CommandStack.Commands;
using Merp.Registry.QueryStack;
using Merp.Registry.Web.Api.Internal.Models.Person;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Api.Internal.WorkerServices
{
    public class PersonControllerWorkerServices
    {
        public IDatabase Database { get; private set; }

        public IBus Bus { get; private set; }

        public IHttpContextAccessor ContextAccessor { get; private set; }

        public PersonControllerWorkerServices(IDatabase database, IBus bus, IHttpContextAccessor contextAccessor)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public IEnumerable<SearchByPatternModel> SearchPeopleByPattern(string query)
        {
            var people = from p in Database.People
                         where p.DisplayName.StartsWith(query)
                         orderby p.DisplayName ascending
                         select new SearchByPatternModel
                         {
                             Id = p.Id,
                             Name = p.DisplayName,
                             OriginalId = p.OriginalId
                         };

            return people.ToArray();
        }

        public async Task RegisterPersonAsync(RegisterModel model)
        {
            var userId = Guid.NewGuid();
            var nationalIdentificationNumber = string.IsNullOrWhiteSpace(model.NationalIdentificationNumber) ? default(string) : model.NationalIdentificationNumber.Trim().ToUpper();

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

        #region Private helpers
        private Guid GetCurrentUserId()
        {
            var userId = ContextAccessor.HttpContext.User.FindFirstValue("sub");
            return Guid.Parse(userId);
        }
        #endregion
    }
}

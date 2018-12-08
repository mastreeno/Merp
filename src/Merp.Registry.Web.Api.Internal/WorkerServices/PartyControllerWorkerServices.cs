using MementoFX;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.QueryStack;
using Merp.Registry.Web.Api.Internal.Models.Party;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Api.Internal.WorkerServices
{
    public class PartyControllerWorkerServices
    {
        public IDatabase Database { get; private set; }

        public IBus Bus { get; private set; }

        public IHttpContextAccessor ContextAccessor { get; private set; }

        public PartyControllerWorkerServices(IDatabase database, IBus bus, IHttpContextAccessor contextAccessor)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public IEnumerable<PartyInfoModel> GetPartyInfoByPattern(string text)
        {
            var model = from p in Database.Parties
                        where p.DisplayName.StartsWith(text)
                        orderby p.DisplayName ascending
                        select new PartyInfoModel { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName };

            return model;
        }

        public PartyInfoModel GetPartyInfoById(Guid partyId)
        {
            var model = (from p in Database.Parties
                         where p.OriginalId == partyId
                         orderby p.DisplayName ascending
                         select new PartyInfoModel { Id = p.Id, OriginalId = p.OriginalId, Name = p.DisplayName }
                         ).Single();

            return model;
        }

        public async Task RegisterPartyAsync(RegisterModel model)
        {
            Command command;
            var userId = GetCurrentUserId();
            var nationalIdentificationNumber = string.IsNullOrWhiteSpace(model.NationalIdentificationNumber) ? default(string) : model.NationalIdentificationNumber.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                command = new RegisterCompanyCommand(
                    userId,
                    model.LastNameOrCompanyName,
                    nationalIdentificationNumber,
                    model.VatNumber,
                    model.Address.Address,
                    model.Address.PostalCode,
                    model.Address.City,
                    model.Address.Province,
                    model.Address.Country,
                    model.Address.Address,
                    model.Address.PostalCode,
                    model.Address.City,
                    model.Address.Province,
                    model.Address.Country,
                    model.Address.Address,
                    model.Address.PostalCode,
                    model.Address.City,
                    model.Address.Province,
                    model.Address.Country,
                    null, null, null, null, null, null);
            }
            else
            {
                command = new RegisterPersonCommand(
                    userId,
                    model.FirstName,
                    model.LastNameOrCompanyName,
                    nationalIdentificationNumber,
                    model.VatNumber,
                    model.Address.Address,
                    model.Address.PostalCode,
                    model.Address.City,
                    model.Address.Province,
                    model.Address.Country,
                    model.Address.Address,
                    model.Address.PostalCode,
                    model.Address.City,
                    model.Address.Province,
                    model.Address.Country,
                    model.Address.Address,
                    model.Address.PostalCode,
                    model.Address.City,
                    model.Address.Province,
                    model.Address.Country,
                    null, null, null, null, null, null);
            }

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

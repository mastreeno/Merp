using Merp.Registry.CommandStack.Commands;
using Merp.Registry.QueryStack;
using Merp.Registry.Web.Api.Public.Models;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Api.Public.WorkerServices
{
    public class PersonControllerWorkerServices
    {
        public IBus Bus { get; private set; }

        public IHttpContextAccessor ContextAccessor { get; private set; }

        public IDatabase Database { get; private set; }

        public PersonControllerWorkerServices(IDatabase database, IBus bus, IHttpContextAccessor contextAccessor)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task ImportPersonAsync(ImportPersonModel model)
        {
            var nationalIdentificationNumber = string.IsNullOrWhiteSpace(model.NationalIdentificationNumber) ? default(string) : model.NationalIdentificationNumber.Trim().ToUpper();

            var command = new ImportPersonCommand(
                model.UserId,
                model.PersonId,
                model.RegistrationDate,
                model.FirstName,
                model.LastName,
                nationalIdentificationNumber,
                model.VatNumber,
                model.Address?.Address,
                model.Address?.City,
                model.Address?.PostalCode,
                model.Address?.Province,
                model.Address?.Country,
                model.PhoneNumber,
                model.MobileNumber,
                model.FaxNumber,
                model.WebsiteAddress,
                model.EmailAddress,
                model.InstantMessaging
            );  

            await Bus.Send(command);
        }

        public async Task SetPersonContactInfoAsync(SetPersonContactInfoModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = model.UserId;

            var cmd = new ChangePersonContactInfoCommand(
                userId,
                model.PersonId,
                model.PhoneNumber,
                model.MobileNumber,
                model.FaxNumber,
                model.WebsiteAddress,
                model.EmailAddress,
                model.InstantMessaging);

            await Bus.Send(cmd);
        }
    }
}

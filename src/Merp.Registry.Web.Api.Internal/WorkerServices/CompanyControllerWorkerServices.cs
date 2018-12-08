using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.Web.Api.Internal.Models.Company;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;

namespace Merp.Registry.Web.Api.Internal.WorkerServices
{
    public class CompanyControllerWorkerServices
    {
        public IBus Bus { get; private set; }

        public IHttpContextAccessor ContextAccessor { get; private set; }

        public CompanyControllerWorkerServices(IBus bus, IHttpContextAccessor contextAccessor)
        {
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task RegisterCompanyAsync(RegisterModel model)
        {
            var userId = GetCurrentUserId();
            var nationalIdentificationNumber = string.IsNullOrWhiteSpace(model.NationalIdentificationNumber) ? default(string) : model.NationalIdentificationNumber.Trim().ToUpper();

            var command = new RegisterCompanyCommand(
                    userId,
                    model.CompanyName,
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

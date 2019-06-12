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
    public class CompanyControllerWorkerServices
    {
        public IBus Bus { get; private set; }

        public IHttpContextAccessor ContextAccessor { get; private set; }

        public IDatabase Database { get; private set; }

        public CompanyControllerWorkerServices(IDatabase database, IBus bus, IHttpContextAccessor contextAccessor)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task AssociateCompanyAdministrativeContact(AssociateCompanyAdministrativeContactModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new AssociateAdministrativeContactToCompanyCommand(
                model.UserId,
                model.CompanyId,
                model.AdministrativeContactId
            );

            await Bus.Send(cmd);
        }

        public async Task AssociateCompanyMainContact(AssociateCompanyMainContactModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var cmd = new AssociateMainContactToCompanyCommand(
                model.UserId,
                model.CompanyId,
                model.MainContactId
            );

            await Bus.Send(cmd);
        }

        public async Task ImportCompanyAsync(ImportCompanyModel model)
        {
            var nationalIdentificationNumber = string.IsNullOrWhiteSpace(model.NationalIdentificationNumber) ? default(string) : model.NationalIdentificationNumber.Trim().ToUpper();

            var legalAddressAddress = model.LegalAddressAddress;
            var legalAddressPostalCode = model.LegalAddressPostalCode;
            var legalAddressCity = model.LegalAddressCity;
            var legalAddressProvince = model.LegalAddressProvince;
            var legalAddressCountry = model.LegalAddressCountry;

            var shippingAddressAddress = model.ShippingAddressAddress;
            var shippingAddressPostalCode = model.ShippingAddressPostalCode;
            var shippingAddressCity = model.ShippingAddressCity;
            var shippingAddressProvince = model.ShippingAddressProvince;
            var shippingAddressCountry = model.ShippingAddressCountry;

            var billingAddressAddress = model.BillingAddressAddress;
            var billingAddressPostalCode = model.BillingAddressPostalCode;
            var billingAddressCity = model.BillingAddressCity;
            var billingAddressProvince = model.BillingAddressProvince;
            var billingAddressCountry = model.BillingAddressCountry;

            var phoneNumber = model.PhoneNumber;
            var faxNumber = model.FaxNumber;
            var websiteAddress = model.WebsiteAddress;
            var emailAddress = model.EmailAddress;

            Guid? mainContactId = default(Guid?);
            Guid? administrativeContactId = default(Guid?);

            var command = new ImportCompanyCommand(
                model.UserId,
                model.CompanyId,
                model.RegistrationDate,
                model.CompanyName,
                nationalIdentificationNumber,
                model.VatNumber,
                model.LegalAddressAddress,
                model.LegalAddressPostalCode,
                model.LegalAddressCity,
                model.LegalAddressProvince,
                model.LegalAddressCountry,
                model.ShippingAddressAddress,
                model.ShippingAddressPostalCode,
                model.ShippingAddressCity,
                model.ShippingAddressProvince,
                model.ShippingAddressCountry,
                model.BillingAddressAddress,
                model.BillingAddressPostalCode,
                model.BillingAddressCity,
                model.BillingAddressProvince,
                model.BillingAddressCountry,
                mainContactId,
                administrativeContactId,
                model.PhoneNumber,
                model.FaxNumber,
                model.WebsiteAddress,
                model.EmailAddress
            );

            await Bus.Send(command);
        }

        public async Task SetCompanyContactInfoAsync(SetCompanyContactInfoModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = model.UserId;

            var cmd = new ChangeCompanyContactInfoCommand(
                userId,
                model.CompanyId,
                model.PhoneNumber,
                model.FaxNumber,
                model.WebsiteAddress,
                model.EmailAddress);

            await Bus.Send(cmd);
        }
    }
}

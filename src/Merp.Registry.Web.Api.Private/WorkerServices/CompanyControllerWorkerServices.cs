using System;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Threading.Tasks;
using MementoFX.Persistence;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.CommandStack.Model;
using Merp.Registry.QueryStack;
using Merp.Registry.Web.Models;
using Merp.Registry.Web.Models.Company;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;

namespace Merp.Registry.Web.WorkerServices
{
    public class CompanyControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; private set; }
        public IRepository Repository { get; private set; }
        public IHttpContextAccessor ContextAccessor { get; private set; }

        public CompanyControllerWorkerServices(IBus bus, IDatabase database, IRepository repository, IHttpContextAccessor contextAccessor)
        {
            this.Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task AddEntryAsync(AddEntryModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var userId = GetCurrentUserId();

            var companyName = model.CompanyName;
            var nationalIdentificationNumber = model.NationalIdentificationNumber;
            var vatNumber = model.VatNumber;

            var legalAddressAddress = model.LegalAddress.Address;
            var legalAddressPostalCode = model.LegalAddress.PostalCode;
            var legalAddressCity = model.LegalAddress.City;
            var legalAddressProvince = model.LegalAddress.Province;
            var legalAddressCountry = model.LegalAddress.Country;

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

            var phoneNumber = model.PhoneNumber;
            var faxNumber = model.FaxNumber;
            var websiteAddress = model.WebsiteAddress;
            var emailAddress = model.EmailAddress;

            Guid? mainContactId = model.MainContact == null ? default(Guid?) : model.MainContact.OriginalId;
            Guid? administrativeContactId = model.AdministrativeContact == null ? default(Guid?) : model.AdministrativeContact.OriginalId;

            var command = new RegisterCompanyCommand(
                userId,
                companyName,
                nationalIdentificationNumber,
                vatNumber,

                legalAddressAddress,
                legalAddressPostalCode,
                legalAddressCity,
                legalAddressProvince,
                legalAddressCountry,

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

                mainContactId,
                administrativeContactId,
                phoneNumber,
                faxNumber,
                websiteAddress,
                emailAddress
                );

            await Bus.Send(command);
        }

        public InfoModel GetInfoViewModel(Guid companyId)
        {
            var company = Repository.GetById<Company>(companyId);
            var model = new InfoModel()
            {
                CompanyUid = company.Id,
                CompanyName = company.CompanyName,
                VatNumber = company.VatNumber,
                NationalIdentificationNumber = company.NationalIdentificationNumber,
                LegalAddress = new Models.PostalAddress(),
                ShippingAddress = new Models.PostalAddress(),
                BillingAddress = new Models.PostalAddress()
            };

            if (company.LegalAddress != null)
            {
                model.LegalAddress.Address = company.LegalAddress.Address;
                model.LegalAddress.City = company.LegalAddress.City;
                model.LegalAddress.Country = company.LegalAddress.Country;
                model.LegalAddress.PostalCode = company.LegalAddress.PostalCode;
                model.LegalAddress.Province = company.LegalAddress.Province;
            }

            if (company.ShippingAddress != null)
            {
                model.ShippingAddress.Address = company.ShippingAddress.Address;
                model.ShippingAddress.City = company.ShippingAddress.City;
                model.ShippingAddress.Country = company.ShippingAddress.Country;
                model.ShippingAddress.PostalCode = company.ShippingAddress.PostalCode;
                model.ShippingAddress.Province = company.ShippingAddress.Province;
            }

            if (company.BillingAddress != null)
            {
                model.BillingAddress.Address = company.BillingAddress.Address;
                model.BillingAddress.City = company.BillingAddress.City;
                model.BillingAddress.Country = company.BillingAddress.Country;
                model.BillingAddress.PostalCode = company.BillingAddress.PostalCode;
                model.BillingAddress.Province = company.BillingAddress.Province;
            }

            if (company.ContactInfo != null)
            {
                model.PhoneNumber = company.ContactInfo.PhoneNumber;
                model.FaxNumber = company.ContactInfo.FaxNumber;
                model.WebsiteAddress = company.ContactInfo.WebsiteAddress;
                model.EmailAddress = company.ContactInfo.EmailAddress;
            }

            if (company.MainContactId.HasValue && company.MainContactId != Guid.Empty)
            {
                model.MainContact = Database.People
                    .Where(p => p.OriginalId == company.MainContactId.Value)
                    .Select(p => new PersonInfo
                    {
                        Id = p.Id,
                        OriginalId = p.OriginalId,
                        Name = p.DisplayName
                    })
                    .Single();
            }

            if (company.AdministrativeContactId.HasValue && company.AdministrativeContactId != Guid.Empty)
            {
                model.AdministrativeContact = Database.People
                    .Where(p => p.OriginalId == company.AdministrativeContactId.Value)
                    .Select(p => new PersonInfo
                    {
                        Id = p.Id,
                        OriginalId = p.OriginalId,
                        Name = p.DisplayName
                    })
                    .Single();
            }

            return model;
        }

        public async Task ChangeNameAsync(Guid companyId, ChangeNameModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var cmd = new ChangeCompanyNameCommand(userId, companyId, model.NewCompanyName, model.EffectiveDate);
            await Bus.Send(cmd);
        }

        public async Task ChangeLegalAddressAsync(Guid companyId, ChangeLegalAddressModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangeCompanyLegalAddressCommand(
                userId,
                companyId,
                model.LegalAddress.Address,
                model.LegalAddress.PostalCode,
                model.LegalAddress.City,
                model.LegalAddress.Province,
                model.LegalAddress.Country,
                effectiveDate);

            await Bus.Send(cmd);
        }

        public async Task ChangeShippingAddressAsync(Guid companyId, ChangeShippingAddressModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangeCompanyShippingAddressCommand(
                userId,
                companyId,
                model.ShippingAddress.Address,
                model.ShippingAddress.PostalCode,
                model.ShippingAddress.City,
                model.ShippingAddress.Province,
                model.ShippingAddress.Country,
                effectiveDate);

            await Bus.Send(cmd);
        }

        public async Task ChangeBillingAddressAsync(Guid companyId, ChangeBillingAddressModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangeCompanyBillingAddressCommand(
                userId,
                companyId,
                model.BillingAddress.Address,
                model.BillingAddress.PostalCode,
                model.BillingAddress.City,
                model.BillingAddress.Province,
                model.BillingAddress.Country,
                effectiveDate);

            await Bus.Send(cmd);
        }

        public async Task ChangeContactInfoAsync(Guid companyId, ChangeContactInfoModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var cmd = new ChangeCompanyContactInfoCommand(userId, companyId, model.PhoneNumber, model.FaxNumber, model.WebsiteAddress, model.EmailAddress);

            await Bus.Send(cmd);
        }

        public async Task AssociateMainContactAsync(Guid companyId, AssociateMainContactModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var cmd = new AssociateMainContactToCompanyCommand(userId, companyId, model.MainContact.OriginalId);

            await Bus.Send(cmd);
        }

        public async Task AssociateAdministrativeContactAsync(Guid companyId, AssociateAdministrativeContactModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var cmd = new AssociateAdministrativeContactToCompanyCommand(userId, companyId, model.AdministrativeContact.OriginalId);

            await Bus.Send(cmd);
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

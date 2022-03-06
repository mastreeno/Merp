using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Threading.Tasks;
using MementoFX.Persistence;
using Merp.Registry.CommandStack.Commands;
using Merp.Registry.CommandStack.Model;
using Merp.Registry.QueryStack;
using Merp.Registry.Web.Models.Person;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;

namespace Merp.Registry.Web.WorkerServices
{
    public class PersonControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; private set; }
        public IRepository Repository { get; private set; }
        public IHttpContextAccessor ContextAccessor { get; private set; }

        public PersonControllerWorkerServices(IBus bus, IDatabase database, IRepository repository, IHttpContextAccessor contextAccessor)
        {
            this.Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task AddEntryAsync(AddEntryModel model)
        {
            var userId = GetCurrentUserId();

            var nationalIdentificationNumber = string.IsNullOrWhiteSpace(model.NationalIdentificationNumber) ? default(string) : model.NationalIdentificationNumber.Trim().ToUpper();

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

            var command = new RegisterPersonCommand(
                userId,
                model.FirstName,
                model.LastName,
                nationalIdentificationNumber,
                model.VatNumber,
                model.LegalAddress.Address,
                model.LegalAddress.City,
                model.LegalAddress.PostalCode,
                model.LegalAddress.Province,
                model.LegalAddress.Country,
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
                model.PhoneNumber,
                model.MobileNumber,
                model.FaxNumber,
                model.WebsiteAddress,
                model.EmailAddress,
                model.InstantMessaging
                );

            await Bus.Send(command);
        }

        public InfoModel GetInfoViewModel(Guid personId)
        {
            var person = Repository.GetById<Person>(personId);
            var model = new InfoModel()
            {
                OriginalId = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                NationalIdentificationNumber = person.NationalIdentificationNumber,
                VatNumber = person.VatNumber
            };

            if (person.LegalAddress != null)
            {
                model.LegalAddress = new Models.PostalAddress
                {
                    Address = person.LegalAddress.Address,
                    City = person.LegalAddress.City,
                    Country = person.LegalAddress.Country,
                    PostalCode = person.LegalAddress.PostalCode,
                    Province = person.LegalAddress.Province
                };
            }

            if (person.ShippingAddress != null)
            {
                model.ShippingAddress = new Models.PostalAddress
                {
                    Address = person.ShippingAddress.Address,
                    City = person.ShippingAddress.City,
                    Country = person.ShippingAddress.Country,
                    PostalCode = person.ShippingAddress.PostalCode,
                    Province = person.ShippingAddress.Province
                };
            }

            if (person.BillingAddress != null)
            {
                model.BillingAddress = new Models.PostalAddress
                {
                    Address = person.BillingAddress.Address,
                    City = person.BillingAddress.City,
                    Country = person.BillingAddress.Country,
                    PostalCode = person.BillingAddress.PostalCode,
                    Province = person.BillingAddress.Province
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
            }

            model.Id = Database.People
                .Where(p => p.OriginalId == person.Id)
                .Select(p => p.Id)
                .Single();

            return model;
        }

        public async Task ChangeLegalAddressAsync(Guid personId, ChangeLegalAddressModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangePersonLegalAddressCommand(
                userId,
                personId,
                model.Address.Address,
                model.Address.PostalCode,
                model.Address.City,
                model.Address.Province,
                model.Address.Country,
                effectiveDate);

            await Bus.Send(cmd);
        }

        public async Task ChangeShippingAddressAsync(Guid personId, ChangeShippingAddressModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangePersonShippingAddressCommand(
                userId,
                personId,
                model.Address.Address,
                model.Address.PostalCode,
                model.Address.City,
                model.Address.Province,
                model.Address.Country,
                effectiveDate);

            await Bus.Send(cmd);
        }

        public async Task ChangeBillingAddressAsync(Guid personId, ChangeBillingAddressModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var effectiveDateTime = model.EffectiveDate;
            var effectiveDate = new DateTime(effectiveDateTime.Year, effectiveDateTime.Month, effectiveDateTime.Day);

            var cmd = new ChangePersonBillingAddressCommand(
                userId,
                personId,
                model.Address.Address,
                model.Address.PostalCode,
                model.Address.City,
                model.Address.Province,
                model.Address.Country,
                effectiveDate);

            await Bus.Send(cmd);
        }

        public async Task ChangeContactInfoAsync(Guid personId, ChangeContactInfoModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = GetCurrentUserId();

            var cmd = new ChangePersonContactInfoCommand(
                userId,
                personId, 
                model.PhoneNumber, 
                model.MobileNumber, 
                model.FaxNumber, 
                model.WebsiteAddress, 
                model.EmailAddress, 
                model.InstantMessaging);

            await Bus.Send(cmd);
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

            return people.ToList();
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

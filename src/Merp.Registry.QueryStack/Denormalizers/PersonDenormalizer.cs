using Merp.Registry.CommandStack.Events;
using Merp.Registry.QueryStack.Model;
using Rebus.Handlers;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Merp.Registry.QueryStack.Denormalizers
{
    public class PersonDenormalizer : 
        IHandleMessages<PersonRegisteredEvent>
    {
        private DbContextOptions<RegistryDbContext> Options;

        public PersonDenormalizer(DbContextOptions<RegistryDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(PersonRegisteredEvent message)
        {
            var p = new Person()
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                OriginalId = message.PersonId,
                DisplayName = $"{message.FirstName} {message.LastName}",
                NationalIdentificationNumber = message.NationalIdentificationNumber,
                VatIndex = message.VatNumber
                //LegalAddress = new PostalAddress
                //{
                //    Address = message.Address,
                //    City = message.City,
                //    Country = message.Country,
                //    PostalCode = message.PostalCode,
                //    Province = message.Province
                //},
                //ShippingAddress = new PostalAddress(),
                //BillingAddress = new PostalAddress(),
                //ContactInfo = new ContactInfo
                //{
                //    PhoneNumber = message.PhoneNumber,
                //    MobileNumber = message.MobileNumber,
                //    FaxNumber = message.FaxNumber,
                //    WebsiteAddress = message.WebsiteAddress,
                //    EmailAddress = message.EmailAddress,
                //    InstantMessaging = message.InstantMessaging
                //}
            };
            using (var context = new RegistryDbContext(Options))
            {
                context.People.Add(p);
                await context.SaveChangesAsync();
            }
        }
    }
}

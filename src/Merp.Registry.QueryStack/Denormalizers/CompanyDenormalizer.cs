using Merp.Registry.CommandStack.Events;
using Merp.Registry.QueryStack.Model;
using Rebus.Handlers;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace Merp.Registry.QueryStack.Denormalizers
{
    public class CompanyDenormalizer : 
        IHandleMessages<CompanyRegisteredEvent>,
        IHandleMessages<CompanyNameChangedEvent>,
        IHandleMessages<CompanyAdministrativeContactAssociatedEvent>,
        IHandleMessages<CompanyMainContactAssociatedEvent>
    {
        private DbContextOptions<RegistryDbContext> Options;

        public CompanyDenormalizer(DbContextOptions<RegistryDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(CompanyRegisteredEvent message)
        {
            var p = new Company()
            {
                CompanyName = message.CompanyName,
                VatIndex = message.VatIndex,
                OriginalId = message.CompanyId,
                NationalIdentificationNumber = message.NationalIdentificationNumber ?? string.Empty,
                LegalAddress = new PostalAddress
                {
                    Address = message.LegalAddressAddress,
                    City = message.LegalAddressCity,
                    Country = message.LegalAddressCountry,
                    PostalCode = message.LegalAddressPostalCode,
                    Province = message.LegalAddressProvince
                },
                ShippingAddress = new PostalAddress()
                {
                    Address = message.ShippingAddressAddress,
                    City = message.ShippingAddressCity,
                    Country = message.ShippingAddressCountry,
                    PostalCode = message.ShippingAddressPostalCode,
                    Province = message.ShippingAddressProvince
                },
                BillingAddress = new PostalAddress()
                {
                    Address = message.BillingAddressAddress,
                    City = message.BillingAddressCity,
                    Country = message.BillingAddressCountry,
                    PostalCode = message.BillingAddressPostalCode,
                    Province = message.BillingAddressProvince
                }
            };
            using (var context = new RegistryDbContext(Options))
            {
                context.Companies.Add(p);
                await context.SaveChangesAsync();
            }
        }

        public async Task Handle(CompanyNameChangedEvent message)
        {
            using (var context = new RegistryDbContext(Options))
            {
                var company = (from c in context.Companies
                               where c.OriginalId == message.CompanyId
                               select c).Single();
                company.CompanyName = message.CompanyName;

                await context.SaveChangesAsync();
            }
        }

        public async Task Handle(CompanyAdministrativeContactAssociatedEvent message)
        {
            using (var context = new RegistryDbContext(Options))
            {
                var company = (from c in context.Companies
                               where c.OriginalId == message.CompanyId
                               select c).Single();
                var person = (from c in context.People
                              where c.OriginalId == message.AdministrativeContactId
                              select c).Single();
                company.AdministrativeContactName = $"{person.FirstName} {person.LastName}";
                company.AdministrativeContactUid = message.AdministrativeContactId;

                await context.SaveChangesAsync();
            }
        }

        public async Task Handle(CompanyMainContactAssociatedEvent message)
        {
            using (var context = new RegistryDbContext(Options))
            {
                var company = (from c in context.Companies
                               where c.OriginalId == message.CompanyId
                               select c).Single();
                var person = (from c in context.People
                              where c.OriginalId == message.MainContactId
                              select c).Single();
                company.MainContactName = $"{person.FirstName} {person.LastName}";
                company.MainContactUid = message.MainContactId;

                await context.SaveChangesAsync();
            }
        }
    }
}

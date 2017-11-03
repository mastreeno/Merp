using Merp.Registry.CommandStack.Events;
using Merp.Registry.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Rebus.Bus;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.QueryStack.Denormalizers
{
    public class PartyDenormalizer :
        IHandleMessages<PartyLegalAddressChangedEvent>,
        IHandleMessages<ContactInfoSetForPartyEvent>,
        IHandleMessages<PersonRegisteredEvent>,
        IHandleMessages<CompanyRegisteredEvent>,
        IHandleMessages<CompanyNameChangedEvent>
    {
        private DbContextOptions<RegistryDbContext> Options;

        public PartyDenormalizer(DbContextOptions<RegistryDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(PartyLegalAddressChangedEvent message)
        {
            using (var context = new RegistryDbContext(Options))
            {
                var legalAddress = new PostalAddress()
                {
                    Address = message.Address,
                    City = message.City,
                    Country = message.Country,
                    PostalCode = message.PostalCode,
                    Province = message.Province
                };
                var party = (from c in context.Parties
                             where c.OriginalId == message.PartyId
                             select c).Single();
                party.LegalAddress = legalAddress;

                await context.SaveChangesAsync();
            }
        }

        public async Task Handle(ContactInfoSetForPartyEvent message)
        {
            using (var context = new RegistryDbContext(Options))
            {
                //var contactInfo = new ContactInfo()
                //{
                //    PhoneNumber = message.PhoneNumber,
                //    MobileNumber = message.MobileNumber,
                //    FaxNumber = message.FaxNumber,
                //    WebsiteAddress = message.WebsiteAddress,
                //    EmailAddress = message.EmailAddress,
                //    InstantMessaging = message.InstantMessaging
                //};
                var party = (from c in context.Parties
                             where c.OriginalId == message.PartyId
                             select c).Single();
                //party.ContactInfo = contactInfo;
                party.PhoneNumber = message.PhoneNumber;
                party.MobileNumber = message.MobileNumber;
                party.FaxNumber = message.FaxNumber;
                party.WebsiteAddress = message.WebsiteAddress;
                party.EmailAddress = message.EmailAddress;
                party.InstantMessaging = message.InstantMessaging;
                party.Pec = message.Pec;
                await context.SaveChangesAsync();
            }
        }

        #region Person
        public async Task Handle(PersonRegisteredEvent message)
        {
            var p = new Party()
            {
                OriginalId = message.PersonId,
                DisplayName = $"{message.FirstName} {message.LastName}",
                NationalIdentificationNumber = message.NationalIdentificationNumber,
                VatIndex = message.VatNumber,
                Type = Party.PartyType.Person,
                LegalAddress = new PostalAddress
                {
                    Address = message.Address,
                    City = message.City,
                    Country = message.Country,
                    PostalCode = message.PostalCode,
                    Province = message.Province
                },
                //ContactInfo = new ContactInfo
                //{
                PhoneNumber = message.PhoneNumber,
                MobileNumber = message.MobileNumber,
                FaxNumber = message.FaxNumber,
                WebsiteAddress = message.WebsiteAddress,
                EmailAddress = message.EmailAddress,
                InstantMessaging = message.InstantMessaging,
                Pec = message.Pec
                //}
            };
            using (var context = new RegistryDbContext(Options))
            {
                context.Parties.Add(p);
                await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Company
        public async Task Handle(CompanyRegisteredEvent message)
        {
            var p = new Party()
            {
                DisplayName = message.CompanyName,
                VatIndex = message.VatIndex,
                OriginalId = message.CompanyId,
                NationalIdentificationNumber = message.NationalIdentificationNumber ?? "",
                Type = Party.PartyType.Company,
                LegalAddress = new PostalAddress
                {
                    Address = message.LegalAddressAddress,
                    City = message.LegalAddressCity,
                    Country = message.LegalAddressCountry,
                    PostalCode = message.LegalAddressPostalCode,
                    Province = message.LegalAddressProvince
                }
            };
            using (var context = new RegistryDbContext(Options))
            {
                context.Parties.Add(p);
                await context.SaveChangesAsync();
            }
        }

        public async Task Handle(CompanyNameChangedEvent message)
        {
            using (var context = new RegistryDbContext(Options))
            {
                var company = (from c in context.Parties
                               where c.OriginalId == message.CompanyId
                               select c).Single();
                company.DisplayName = message.CompanyName;

                await context.SaveChangesAsync();
            }
        }
        #endregion
    }
}

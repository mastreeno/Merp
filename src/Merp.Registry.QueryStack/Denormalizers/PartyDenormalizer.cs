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
        IHandleMessages<PartyBillingAddressChangedEvent>,
        IHandleMessages<PartyLegalAddressChangedEvent>,
        IHandleMessages<PartyShippingAddressChangedEvent>,
        IHandleMessages<ContactInfoSetForPartyEvent>,
        IHandleMessages<PartyUnlistedEvent>
    {
        private DbContextOptions<RegistryDbContext> Options;

        public PartyDenormalizer(DbContextOptions<RegistryDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(PartyBillingAddressChangedEvent message)
        {
            using (var context = new RegistryDbContext(Options))
            {
                var billingAddress = new PostalAddress()
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
                party.BillingAddress = billingAddress;

                await context.SaveChangesAsync();
            }
        }
        public async Task Handle(PartyShippingAddressChangedEvent message)
        {
            using (var context = new RegistryDbContext(Options))
            {
                var shippingAddress = new PostalAddress()
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
                party.ShippingAddress = shippingAddress;

                await context.SaveChangesAsync();
            }
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
                var party = (from c in context.Parties
                             where c.OriginalId == message.PartyId
                             select c).Single();
                party.PhoneNumber = message.PhoneNumber;
                party.MobileNumber = message.MobileNumber;
                party.FaxNumber = message.FaxNumber;
                party.WebsiteAddress = message.WebsiteAddress;
                party.EmailAddress = message.EmailAddress;
                party.InstantMessaging = message.InstantMessaging;
                //party.ContactInfo = new ContactInfo
                //{
                //    PhoneNumber = message.PhoneNumber,
                //    MobileNumber = message.MobileNumber,
                //    FaxNumber = message.FaxNumber,
                //    WebsiteAddress = message.WebsiteAddress,
                //    EmailAddress = message.EmailAddress,
                //    InstantMessaging = message.InstantMessaging
                //};
                await context.SaveChangesAsync();
            }
        }

        public async Task Handle(PartyUnlistedEvent message)
        {
            using (var context = new RegistryDbContext(Options))
            {
                var party = context.Parties.Single(p => p.OriginalId == message.PartyId);
                party.Unlisted = true;

                await context.SaveChangesAsync();
            }
        }
    }
}

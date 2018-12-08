using MementoFX.Domain;
using Merp.Registry.CommandStack.Model;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpTestsEx;
using System.Threading.Tasks;
using Merp.Registry.CommandStack.Events;

namespace Merp.Registry.CommandStack.Tests.Model
{
    
    public class CompanyEventsFixture
    {

        [Fact]
        public void ChangeShippingAddress_should_raise_a_ShippingAddressSetForPartyEvent()
        {
            var userId = Guid.NewGuid();
            var companyName = "Mastreeno";
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var company = Company.Factory.CreateNewEntry(companyName, vatNumber, nationalIdentificationNumber, 
                null, null, null, null, null,
                null, null, null, null, null,
                null, null, null, null, null, userId);
            var effectiveDate = DateTime.MaxValue;
            company.ChangeShippingAddress(address, city, postalCode, province, country, effectiveDate, userId);
            var uncommittedEvent = ((IAggregate)company).GetUncommittedEvents();
            Assert.Equal(2, uncommittedEvent.Count());

            var actualEvent = uncommittedEvent.Last() as PartyShippingAddressChangedEvent;
            Assert.NotNull(actualEvent);
            Assert.Equal(address, actualEvent.Address);
            Assert.Equal(city, actualEvent.City);
            Assert.Equal(postalCode, actualEvent.PostalCode);
            Assert.Equal(province, actualEvent.Province);
            Assert.Equal(country, actualEvent.Country);

        }

        [Fact]
        public void ChangeBillingAddress_should_raise_a_BillingAddressSetForPartyEvent()
        {
            var userId = Guid.NewGuid();
            var companyName = "Mastreeno";
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var company = Company.Factory.CreateNewEntry(companyName, vatNumber, nationalIdentificationNumber, 
                null, null, null, null, null,
                null, null, null, null, null,
                null, null, null, null, null, userId);
            var effectiveDate = DateTime.MaxValue;
            company.ChangeBillingAddress(address, city, postalCode, province, country, effectiveDate, userId);
            var uncommittedEvent = ((IAggregate)company).GetUncommittedEvents();
            Assert.Equal(2, uncommittedEvent.Count());

            var actualEvent = uncommittedEvent.Last() as PartyBillingAddressChangedEvent;
            Assert.NotNull(actualEvent);
            Assert.Equal(address, actualEvent.Address);
            Assert.Equal(city, actualEvent.City);
            Assert.Equal(postalCode, actualEvent.PostalCode);
            Assert.Equal(province, actualEvent.Province);
            Assert.Equal(country, actualEvent.Country);

        }

        [Fact]
        public void ChangeLegalAddress_should_raise_a_LegalAddressSetForPartyEvent()
        {
            var userId = Guid.NewGuid();
            var companyName = "Mastreeno";
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var company = Company.Factory.CreateNewEntry(companyName, vatNumber, nationalIdentificationNumber, 
                null, null, null, null, null,
                null, null, null, null, null,
                null, null, null, null, null, userId);
            var effectiveDate = DateTime.Now;
            company.ChangeLegalAddress(address, city, postalCode, province, country, effectiveDate, userId);
            var uncommittedEvent = ((IAggregate)company).GetUncommittedEvents();
            Assert.Equal(2, uncommittedEvent.Count());

            var actualEvent = uncommittedEvent.Last() as PartyLegalAddressChangedEvent;
            Assert.NotNull(actualEvent);
            Assert.Equal(address, actualEvent.Address);
            Assert.Equal(city, actualEvent.City);
            Assert.Equal(postalCode, actualEvent.PostalCode);
            Assert.Equal(province, actualEvent.Province);
            Assert.Equal(country, actualEvent.Country);

        }
    }
}

using Memento.Domain;
using Merp.Registry.CommandStack.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpTestsEx;
using System.Threading.Tasks;
using Merp.Registry.CommandStack.Events;

namespace Merp.Registry.CommandStack.Tests.Model
{
    [TestFixture]
    public class CompanyEventsFixture
    {

        [Test]
        public void ChangeShippingAddress_should_raise_a_ShippingAddressSetForPartyEvent()
        {
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
                null, null, null, null, null);
            var effectiveDate = DateTime.MaxValue;
            company.ChangeShippingAddress(address, city, postalCode, province, country, effectiveDate);
            var uncommittedEvent = ((IAggregate)company).GetUncommittedEvents();
            Assert.AreEqual(2, uncommittedEvent.Count());

            var actualEvent = uncommittedEvent.Last() as PartyShippingAddressChangedEvent;
            Assert.NotNull(actualEvent);
            Assert.AreEqual(address, actualEvent.Address);
            Assert.AreEqual(city, actualEvent.City);
            Assert.AreEqual(postalCode, actualEvent.PostalCode);
            Assert.AreEqual(province, actualEvent.Province);
            Assert.AreEqual(country, actualEvent.Country);

        }

        [Test]
        public void ChangeBillingAddress_should_raise_a_BillingAddressSetForPartyEvent()
        {
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
                null, null, null, null, null);
            var effectiveDate = DateTime.MaxValue;
            company.ChangeBillingAddress(address, city, postalCode, province, country, effectiveDate);
            var uncommittedEvent = ((IAggregate)company).GetUncommittedEvents();
            Assert.AreEqual(2, uncommittedEvent.Count());

            var actualEvent = uncommittedEvent.Last() as PartyBillingAddressChangedEvent;
            Assert.NotNull(actualEvent);
            Assert.AreEqual(address, actualEvent.Address);
            Assert.AreEqual(city, actualEvent.City);
            Assert.AreEqual(postalCode, actualEvent.PostalCode);
            Assert.AreEqual(province, actualEvent.Province);
            Assert.AreEqual(country, actualEvent.Country);

        }

        [Test]
        public void ChangeLegalAddress_should_raise_a_LegalAddressSetForPartyEvent()
        {
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
                null, null, null, null, null);
            var effectiveDate = DateTime.Now;
            company.ChangeLegalAddress(address, city, postalCode, province, country, effectiveDate);
            var uncommittedEvent = ((IAggregate)company).GetUncommittedEvents();
            Assert.AreEqual(2, uncommittedEvent.Count());

            var actualEvent = uncommittedEvent.Last() as PartyLegalAddressChangedEvent;
            Assert.NotNull(actualEvent);
            Assert.AreEqual(address, actualEvent.Address);
            Assert.AreEqual(city, actualEvent.City);
            Assert.AreEqual(postalCode, actualEvent.PostalCode);
            Assert.AreEqual(province, actualEvent.Province);
            Assert.AreEqual(country, actualEvent.Country);

        }
    }
}

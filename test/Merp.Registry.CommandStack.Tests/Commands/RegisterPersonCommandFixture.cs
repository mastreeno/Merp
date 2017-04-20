using Merp.Registry.CommandStack.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Tests.Commands
{
    [TestFixture]
    public class RegisterPersonCommandFixture
    {
        [Test]
        public void Ctor_should_properly_initialise_instance()
        {
            var personId = Guid.Empty;
            var firstName = "Martin";
            var lastName = "Gore";
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var sut = new RegisterPersonCommand(firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country);
            Assert.AreEqual(personId, sut.PersonId);
            Assert.AreEqual(firstName, sut.FirstName);
            Assert.AreEqual(lastName, sut.LastName);
            Assert.AreEqual(nationalIdentificationNumber, sut.NationalIdentificationNumber);
            Assert.AreEqual(vatNumber, sut.VatNumber);
            Assert.AreEqual(address, sut.Address);
            Assert.AreEqual(city, sut.City);
            Assert.AreEqual(postalCode, sut.PostalCode);
            Assert.AreEqual(province, sut.Province);
            Assert.AreEqual(country, sut.Country);
        }
    }
}

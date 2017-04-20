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
    public class RegisterCompanyCommandFixture
    {
        [Test]
        public void Ctor_should_properly_initialise_instance()
        {
            var companyId = Guid.Empty;
            var companyName = "Mastreeno ltd";
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var command = new RegisterCompanyCommand(companyName, nationalIdentificationNumber, vatNumber, address, postalCode, city, province, country, address, postalCode, city, province, country, address, postalCode, city, province, country);
            Assert.AreEqual(companyId, command.CompanyId);
            Assert.AreEqual(companyName, command.CompanyName);
            Assert.AreEqual(nationalIdentificationNumber, command.NationalIdentificationNumber);
            Assert.AreEqual(vatNumber, command.VatNumber);
            Assert.AreEqual(address, command.LegalAddressAddress);
            Assert.AreEqual(city, command.LegalAddressCity);
            Assert.AreEqual(postalCode, command.LegalAddressPostalCode);
            Assert.AreEqual(province, command.LegalAddressProvince);
            Assert.AreEqual(country, command.LegalAddressCountry);
        }
    }
}

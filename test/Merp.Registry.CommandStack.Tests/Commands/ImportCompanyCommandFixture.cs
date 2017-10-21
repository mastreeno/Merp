using Merp.Registry.CommandStack.Commands;
using Xunit;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Tests.Commands
{
    
    public class ImportCompanyCommandFixture
    {
        [Fact]
        public void Ctor_should_properly_initialise_instance()
        {
            var companyId = Guid.NewGuid();
            var companyName = "Mastreeno ltd";
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var mainContactId = Guid.NewGuid();
            var administrativeContactId = Guid.NewGuid();
            var phoneNumber = "0123456789";
            var faxNumber = "012345679";
            var websiteAddress = "www.info.com";
            var emailAddress = "user@info.com";

            var command = new ImportCompanyCommand(companyId, DateTime.Now, companyName, nationalIdentificationNumber, vatNumber, address, postalCode, city, province, country, address, postalCode, city, province, country, address, postalCode, city, province, country, mainContactId, administrativeContactId, phoneNumber, faxNumber, websiteAddress, emailAddress);
            Assert.Equal(companyId, command.CompanyId);
            Assert.Equal(companyName, command.CompanyName);
            Assert.Equal(nationalIdentificationNumber, command.NationalIdentificationNumber);
            Assert.Equal(vatNumber, command.VatNumber);
            Assert.Equal(address, command.LegalAddressAddress);
            Assert.Equal(city, command.LegalAddressCity);
            Assert.Equal(postalCode, command.LegalAddressPostalCode);
            Assert.Equal(province, command.LegalAddressProvince);
            Assert.Equal(country, command.LegalAddressCountry);
            Assert.Equal(mainContactId, command.MainContactId);
            Assert.Equal(administrativeContactId, command.AdministrativeContactId);
            Assert.Equal(phoneNumber, command.PhoneNumber);
            Assert.Equal(faxNumber, command.FaxNumber);
            Assert.Equal(websiteAddress, command.WebsiteAddress);
            Assert.Equal(emailAddress, command.EmailAddress);
        }

        [Fact]
        public void Ctor_should_throw_on_empty_companyId()
        {
            var companyId = Guid.Empty;
            var companyName = string.Empty;
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var mainContactId = Guid.NewGuid();
            var administrativeContactId = Guid.NewGuid();
            var phoneNumber = "0123456789";
            var faxNumber = "012345679";
            var websiteAddress = "www.info.com";
            var emailAddress = "user@info.com";

            Executing.This(
                () => new ImportCompanyCommand(companyId, DateTime.Now, companyName, nationalIdentificationNumber, vatNumber, address, postalCode, city, province, country, address, postalCode, city, province, country, address, postalCode, city, province, country, mainContactId, administrativeContactId, phoneNumber, faxNumber, websiteAddress, emailAddress)
            )
            .Should()
            .Throw<ArgumentException>()
            .And
            .ValueOf
            .ParamName
            .Should()
            .Be
            .EqualTo("companyId");
        }

        [Fact]
        public void Ctor_should_throw_on_null_companyName()
        {
            var companyId = Guid.NewGuid();
            string companyName = null;
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var mainContactId = Guid.NewGuid();
            var administrativeContactId = Guid.NewGuid();
            var phoneNumber = "0123456789";
            var faxNumber = "012345679";
            var websiteAddress = "www.info.com";
            var emailAddress = "user@info.com";

            Executing.This(
                () => new ImportCompanyCommand(companyId, DateTime.Now, companyName, nationalIdentificationNumber, vatNumber, address, postalCode, city, province, country, address, postalCode, city, province, country, address, postalCode, city, province, country, mainContactId, administrativeContactId, phoneNumber, faxNumber, websiteAddress, emailAddress)
            )
            .Should()
            .Throw<ArgumentException>()
            .And
            .ValueOf
            .ParamName
            .Should()
            .Be
            .EqualTo("companyName");
        }

        [Fact]
        public void Ctor_should_throw_on_empty_companyName()
        {
            var companyId = Guid.NewGuid();
            var companyName = string.Empty;
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var mainContactId = Guid.NewGuid();
            var administrativeContactId = Guid.NewGuid();
            var phoneNumber = "0123456789";
            var faxNumber = "012345679";
            var websiteAddress = "www.info.com";
            var emailAddress = "user@info.com";

            Executing.This(
                () => new ImportCompanyCommand(companyId, DateTime.Now, companyName, nationalIdentificationNumber, vatNumber, address, postalCode, city, province, country, address, postalCode, city, province, country, address, postalCode, city, province, country, mainContactId, administrativeContactId, phoneNumber, faxNumber, websiteAddress, emailAddress)
            )
            .Should()
            .Throw<ArgumentException>()
            .And
            .ValueOf
            .ParamName
            .Should()
            .Be
            .EqualTo("companyName");
        }

        [Fact]
        public void Ctor_should_throw_on_null_vatNumber()
        {
            var companyId = Guid.NewGuid();
            var companyName = "Mastreeno ltd";
            var nationalIdentificationNumber = "FAKE";
            string vatNumber = null;
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var mainContactId = Guid.NewGuid();
            var administrativeContactId = Guid.NewGuid();
            var phoneNumber = "0123456789";
            var faxNumber = "012345679";
            var websiteAddress = "www.info.com";
            var emailAddress = "user@info.com";

            Executing.This(
                () => new ImportCompanyCommand(companyId, DateTime.Now, companyName, nationalIdentificationNumber, vatNumber, address, postalCode, city, province, country, address, postalCode, city, province, country, address, postalCode, city, province, country, mainContactId, administrativeContactId, phoneNumber, faxNumber, websiteAddress, emailAddress)
            )
            .Should()
            .Throw<ArgumentException>()
            .And
            .ValueOf
            .ParamName
            .Should()
            .Be
            .EqualTo("vatNumber");
        }

        [Fact]
        public void Ctor_should_throw_on_empty_vatNumber()
        {
            var companyId = Guid.NewGuid();
            var companyName = "Mastreeno ltd";
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = string.Empty;
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var mainContactId = Guid.NewGuid();
            var administrativeContactId = Guid.NewGuid();
            var phoneNumber = "0123456789";
            var faxNumber = "012345679";
            var websiteAddress = "www.info.com";
            var emailAddress = "user@info.com";

            Executing.This(
                () => new ImportCompanyCommand(companyId, DateTime.Now, companyName, nationalIdentificationNumber, vatNumber, address, postalCode, city, province, country, address, postalCode, city, province, country, address, postalCode, city, province, country, mainContactId, administrativeContactId, phoneNumber, faxNumber, websiteAddress, emailAddress)
            )
            .Should()
            .Throw<ArgumentException>()
            .And
            .ValueOf
            .ParamName
            .Should()
            .Be
            .EqualTo("vatNumber");
        }
    }
}

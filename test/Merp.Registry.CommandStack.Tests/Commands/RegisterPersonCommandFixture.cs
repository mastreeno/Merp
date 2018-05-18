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
    public class RegisterPersonCommandFixture
    {
        [Fact]
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
            var phoneNumber = "405040420";
            var mobileNumber = "527452042";
            var faxNumber = "0405763872";
            var websiteAddress = "http://www.info.com";
            var emailAddress = "martin@gore.com";
            var instantMessaging = "@im";
            var skype = "skypeuser";

            var sut = new RegisterPersonCommand(firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging, skype);
            Assert.Equal(personId, sut.PersonId);
            Assert.Equal(firstName, sut.FirstName);
            Assert.Equal(lastName, sut.LastName);
            Assert.Equal(nationalIdentificationNumber, sut.NationalIdentificationNumber);
            Assert.Equal(vatNumber, sut.VatNumber);
            Assert.Equal(address, sut.Address);
            Assert.Equal(city, sut.City);
            Assert.Equal(postalCode, sut.PostalCode);
            Assert.Equal(province, sut.Province);
            Assert.Equal(country, sut.Country);
            Assert.Equal(phoneNumber, sut.PhoneNumber);
            Assert.Equal(mobileNumber, sut.MobileNumber);
            Assert.Equal(faxNumber, sut.FaxNumber);
            Assert.Equal(websiteAddress, sut.WebsiteAddress);
            Assert.Equal(emailAddress, sut.EmailAddress);
            Assert.Equal(instantMessaging, sut.InstantMessaging);
            Assert.Equal(skype, sut.Skype);
        }

        [Fact]
        public void Ctor_should_throw_on_null_firstName()
        {
            var personId = Guid.NewGuid();
            string firstName = null;
            var lastName = "Gore";
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var phoneNumber = "405040420";
            var mobileNumber = "527452042";
            var faxNumber = "0405763872";
            var websiteAddress = "http://www.info.com";
            var emailAddress = "martin@gore.com";
            var instantMessaging = "@im";
            var skype = "skypeuser";

            Executing.This(
                () => new RegisterPersonCommand(firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging, skype)
            )
            .Should()
            .Throw<ArgumentException>()
            .And
            .ValueOf
            .ParamName
            .Should()
            .Be
            .EqualTo("firstName");
        }

        [Fact]
        public void Ctor_should_throw_on_null_lastName()
        {
            var personId = Guid.NewGuid();
            var firstName = "Martin";
            string lastName = null;
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            var phoneNumber = "405040420";
            var mobileNumber = "527452042";
            var faxNumber = "0405763872";
            var websiteAddress = "http://www.info.com";
            var emailAddress = "martin@gore.com";
            var instantMessaging = "@im";
            var skype = "skypeuser";

            Executing.This(
                () => new RegisterPersonCommand(firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging, skype)
            )
            .Should()
            .Throw<ArgumentException>()
            .And
            .ValueOf
            .ParamName
            .Should()
            .Be
            .EqualTo("lastName");
        }
    }
}

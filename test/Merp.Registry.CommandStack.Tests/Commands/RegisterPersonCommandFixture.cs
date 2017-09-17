using Merp.Registry.CommandStack.Commands;
using NUnit.Framework;
using SharpTestsEx;
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
            var phoneNumber = "405040420";
            var mobileNumber = "527452042";
            var faxNumber = "0405763872";
            var websiteAddress = "http://www.info.com";
            var emailAddress = "martin@gore.com";
            var instantMessaging = "@im";

            var sut = new RegisterPersonCommand(firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging);
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
            Assert.AreEqual(phoneNumber, sut.PhoneNumber);
            Assert.AreEqual(mobileNumber, sut.MobileNumber);
            Assert.AreEqual(faxNumber, sut.FaxNumber);
            Assert.AreEqual(websiteAddress, sut.WebsiteAddress);
            Assert.AreEqual(emailAddress, sut.EmailAddress);
            Assert.AreEqual(instantMessaging, sut.InstantMessaging);
        }

        [Test]
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

            Executing.This(
                () => new RegisterPersonCommand(firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging)
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

        [Test]
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

            Executing.This(
                () => new RegisterPersonCommand(firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging)
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

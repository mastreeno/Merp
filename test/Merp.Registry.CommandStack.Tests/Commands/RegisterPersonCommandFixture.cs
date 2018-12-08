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
            var userId = Guid.NewGuid();
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
            var shippingAddressAddress = "Via Torino 51";
            var shippingAddressCity = "Milan";
            var shippingAddressPostalCode = "20123";
            var shippingAddressProvince = "MI";
            var shippingAddressCountry = "Italy";
            var billingAddressAddress = "Via Torino 51";
            var billingAddressCity = "Milan";
            var billingAddressPostalCode = "20123";
            var billingAddressProvince = "MI";
            var billingAddressCountry = "Italy";
            var phoneNumber = "405040420";
            var mobileNumber = "527452042";
            var faxNumber = "0405763872";
            var websiteAddress = "http://www.info.com";
            var emailAddress = "martin@gore.com";
            var instantMessaging = "@im";

            var sut = new RegisterPersonCommand(userId, firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country, 
                shippingAddressAddress, shippingAddressPostalCode, shippingAddressCity, shippingAddressProvince, shippingAddressCountry,
                billingAddressAddress, billingAddressPostalCode, billingAddressCity, billingAddressProvince, billingAddressCountry, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging);

            Assert.Equal(personId, sut.PersonId);
            Assert.Equal(firstName, sut.FirstName);
            Assert.Equal(lastName, sut.LastName);
            Assert.Equal(nationalIdentificationNumber, sut.NationalIdentificationNumber);
            Assert.Equal(vatNumber, sut.VatNumber);
            Assert.Equal(address, sut.LegalAddressAddress);
            Assert.Equal(city, sut.LegalAddressCity);
            Assert.Equal(postalCode, sut.LegalAddressPostalCode);
            Assert.Equal(province, sut.LegalAddressProvince);
            Assert.Equal(country, sut.LegalAddressCountry);
            Assert.Equal(phoneNumber, sut.PhoneNumber);
            Assert.Equal(mobileNumber, sut.MobileNumber);
            Assert.Equal(faxNumber, sut.FaxNumber);
            Assert.Equal(websiteAddress, sut.WebsiteAddress);
            Assert.Equal(emailAddress, sut.EmailAddress);
            Assert.Equal(instantMessaging, sut.InstantMessaging);
        }

        [Fact]
        public void Ctor_should_throw_on_null_firstName()
        {
            var userId = Guid.NewGuid();
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
            var shippingAddressAddress = "Via Torino 51";
            var shippingAddressCity = "Milan";
            var shippingAddressPostalCode = "20123";
            var shippingAddressProvince = "MI";
            var shippingAddressCountry = "Italy";
            var billingAddressAddress = "Via Torino 51";
            var billingAddressCity = "Milan";
            var billingAddressPostalCode = "20123";
            var billingAddressProvince = "MI";
            var billingAddressCountry = "Italy";
            var phoneNumber = "405040420";
            var mobileNumber = "527452042";
            var faxNumber = "0405763872";
            var websiteAddress = "http://www.info.com";
            var emailAddress = "martin@gore.com";
            var instantMessaging = "@im";

            Executing.This(
                () => new RegisterPersonCommand(userId, firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country,
                shippingAddressAddress, shippingAddressPostalCode, shippingAddressCity, shippingAddressProvince, shippingAddressCountry,
                billingAddressAddress, billingAddressPostalCode, billingAddressCity, billingAddressProvince, billingAddressCountry, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging)
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
            var userId = Guid.NewGuid();
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
            var shippingAddressAddress = "Via Torino 51";
            var shippingAddressCity = "Milan";
            var shippingAddressPostalCode = "20123";
            var shippingAddressProvince = "MI";
            var shippingAddressCountry = "Italy";
            var billingAddressAddress = "Via Torino 51";
            var billingAddressCity = "Milan";
            var billingAddressPostalCode = "20123";
            var billingAddressProvince = "MI";
            var billingAddressCountry = "Italy";
            var phoneNumber = "405040420";
            var mobileNumber = "527452042";
            var faxNumber = "0405763872";
            var websiteAddress = "http://www.info.com";
            var emailAddress = "martin@gore.com";
            var instantMessaging = "@im";

            Executing.This(
                () => new RegisterPersonCommand(userId, firstName, lastName, nationalIdentificationNumber, vatNumber, address, city, postalCode, province, country,
                shippingAddressAddress, shippingAddressPostalCode, shippingAddressCity, shippingAddressProvince, shippingAddressCountry,
                billingAddressAddress, billingAddressPostalCode, billingAddressCity, billingAddressProvince, billingAddressCountry, phoneNumber, mobileNumber, faxNumber, websiteAddress, emailAddress, instantMessaging)
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

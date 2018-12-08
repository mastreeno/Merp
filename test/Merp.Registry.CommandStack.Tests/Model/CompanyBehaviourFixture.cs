using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SharpTestsEx;
using Merp.Registry.CommandStack.Model;

namespace Merp.Registry.CommandStack.Tests.Model
{
    
    public class CompanyBehaviourFixture
    {       
        [Fact]
        public void ChangeLegalAddress_should_set_LegalAddress_accordingly_to_inputs()
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

            Assert.NotNull(company.LegalAddress);
            Assert.Equal(address, company.LegalAddress.Address);
            Assert.Equal(city, company.LegalAddress.City);
            Assert.Equal(postalCode, company.LegalAddress.PostalCode);
            Assert.Equal(province, company.LegalAddress.Province);
            Assert.Equal(country, company.LegalAddress.Country);
        }

        [Fact]
        public void ChangeBillingAddress_should_set_LegalAddress_accordingly_to_inputs()
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

            Assert.NotNull(company.BillingAddress);
            Assert.Equal(address, company.BillingAddress.Address);
            Assert.Equal(city, company.BillingAddress.City);
            Assert.Equal(postalCode, company.BillingAddress.PostalCode);
            Assert.Equal(province, company.BillingAddress.Province);
            Assert.Equal(country, company.BillingAddress.Country);
        }

        [Fact]
        public void ChangeShippingAddress_should_set_LegalAddress_accordingly_to_inputs()
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

            Assert.NotNull(company.ShippingAddress);
            Assert.Equal(address, company.ShippingAddress.Address);
            Assert.Equal(city, company.ShippingAddress.City);
            Assert.Equal(postalCode, company.ShippingAddress.PostalCode);
            Assert.Equal(province, company.ShippingAddress.Province);
            Assert.Equal(country, company.ShippingAddress.Country);
        }
        

    }
}

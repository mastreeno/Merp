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
    
    public class CompanyFixture
    {
        [Fact]
        public void Factory_should_throw_ArgumentException_on_null_companyName()
        {
            var userId = Guid.NewGuid();
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            Executing.This(() => Company.Factory.CreateNewEntry(null, vatNumber, nationalIdentificationNumber, 
                null, null, null, null, null,
                null, null, null, null, null,
                null, null, null, null, null, userId))
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
        public void Factory_should_throw_ArgumentException_on_blank_companyName()
        {
            var userId = Guid.NewGuid();
            var nationalIdentificationNumber = "FAKE";
            var vatNumber = "123";
            var address = "Via Torino 51";
            var city = "Milan";
            var postalCode = "20123";
            var province = "MI";
            var country = "Italy";
            Executing.This(() => Company.Factory.CreateNewEntry("", vatNumber, nationalIdentificationNumber, 
                null, null, null, null, null,
                null, null, null, null, null,
                null, null, null, null, null, userId))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("companyName");
        }

        //[Fact]
        //public void Factory_should_throw_ArgumentException_on_null_vatNumber()
        //{
        //    var companyName = "Mastreeno ltd";
        //    var nationalIdentificationNumber = "FAKE";
        //    var address = "Via Torino 51";
        //    var city = "Milan";
        //    var postalCode = "20123";
        //    var province = "MI";
        //    var country = "Italy";
        //    Executing.This(() => Company.Factory.CreateNewEntry(companyName, null, nationalIdentificationNumber, 
        //        null, null, null, null, null,
        //        null, null, null, null, null,
        //        null, null, null, null, null))
        //        .Should()
        //        .Throw<ArgumentException>()
        //        .And
        //        .ValueOf
        //        .ParamName
        //        .Should()
        //        .Be
        //        .EqualTo("vatNumber");
        //}

        //[Fact]
        //public void Factory_should_throw_ArgumentException_on_blank_vatNumber()
        //{
        //    var companyName = "Mastreeno ltd";
        //    var nationalIdentificationNumber = "FAKE";
        //    var address = "Via Torino 51";
        //    var city = "Milan";
        //    var postalCode = "20123";
        //    var province = "MI";
        //    var country = "Italy";
        //    Executing.This(() => Company.Factory.CreateNewEntry(companyName, "", nationalIdentificationNumber, 
        //        null, null, null, null, null,
        //        null, null, null, null, null,
        //        null, null, null, null, null))
        //        .Should()
        //        .Throw<ArgumentException>()
        //        .And
        //        .ValueOf
        //        .ParamName
        //        .Should()
        //        .Be
        //        .EqualTo("vatNumber");
        //}
    }
}

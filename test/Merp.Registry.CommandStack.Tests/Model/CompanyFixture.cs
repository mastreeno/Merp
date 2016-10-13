using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpTestsEx;
using Merp.Registry.CommandStack.Model;

namespace Merp.Registry.CommandStack.Tests.Model
{
    [TestFixture]
    public class CompanyFixture
    {
        [Test]
        public void Factory_should_throw_ArgumentException_on_null_companyName()
        {
            Executing.This(() => Company.Factory.CreateNewEntry(null, "GB"))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("companyName");
        }

        [Test]
        public void Factory_should_throw_ArgumentException_on_blank_companyName()
        {
            Executing.This(() => Company.Factory.CreateNewEntry("", "GB"))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("companyName");
        }

        [Test]
        public void Factory_should_throw_ArgumentException_on_null_vatIndex()
        {
            Executing.This(() => Company.Factory.CreateNewEntry("Mastreeno ltd", ""))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("vatIndex");
        }

        [Test]
        public void Factory_should_throw_ArgumentException_on_blank_vatIndex()
        {
            Executing.This(() => Company.Factory.CreateNewEntry("Mastreeno ltd", null))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("vatIndex");
        }
    }
}

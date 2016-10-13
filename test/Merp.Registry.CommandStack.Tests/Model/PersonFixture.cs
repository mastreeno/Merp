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
    public class PersonFixture
    {
        [Test]
        public void Factory_should_throw_ArgumentException_on_null_firstName()
        {
            Executing.This(() => Person.Factory.CreateNewEntry(null, "Saltarello", DateTime.Now))
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
        public void Factory_should_throw_ArgumentException_on_blank_firstName()
        {
            Executing.This(() => Person.Factory.CreateNewEntry("", "Saltarello", DateTime.Now))
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
        public void Factory_should_throw_ArgumentException_on_null_lastName()
        {
            Executing.This(() => Person.Factory.CreateNewEntry("Andrea", null, DateTime.Now))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("lastName");
        }

        [Test]
        public void Factory_should_throw_ArgumentException_on_blank_lastName()
        {
            Executing.This(() => Person.Factory.CreateNewEntry("Andrea", "", DateTime.Now))
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

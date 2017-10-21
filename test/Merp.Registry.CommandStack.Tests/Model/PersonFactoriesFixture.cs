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
    
    public class PersonFactoriesFixture
    {
        [Fact]
        public void CreateNewEntry_should_throw_ArgumentException_on_null_firstName()
        {
            Executing.This(() => Person.Factory.CreateNewEntry(null, "Saltarello", "FAKE", "FAKE", null, null, null, null, null, null, null, null, null, null, null))
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
        public void CreateNewEntry_should_throw_ArgumentException_on_blank_firstName()
        {
            Executing.This(() => Person.Factory.CreateNewEntry("", "Saltarello", "FAKE", "FAKE", null, null, null, null, null, null, null, null, null, null, null))
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
        public void CreateNewEntry_should_throw_ArgumentException_on_null_lastName()
        {
            Executing.This(() => Person.Factory.CreateNewEntry("Andrea", null, "FAKE", "FAKE", null, null, null, null, null, null, null, null, null, null, null))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("lastName");
        }

        [Fact]
        public void CreateNewEntry_should_throw_ArgumentException_on_blank_lastName()
        {
            Executing.This(() => Person.Factory.CreateNewEntry("Andrea", "", "FAKE", "FAKE", null, null, null, null, null, null, null, null, null, null, null))
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

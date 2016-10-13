using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTestsEx;
using Merp.Accountancy.CommandStack.Model;

namespace Merp.Accountancy.CommandStack.Tests.Model
{
    [TestFixture]
    public class MoneyFixture
    {
        [Test]
        public void Ctor_should_throw_ArgumentException_on_empty_Currency()
        {
            Executing.This(() => new Money(42, string.Empty))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("currency");
        }

        [Test]
        public void Ctor_should_throw_ArgumentException_on_null_Currency()
        {
            Executing.This(() => new Money(42, null))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("currency");
        }
    }
}

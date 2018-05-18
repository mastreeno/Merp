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
    public class PositiveMoneyFixture
    {
        [Xunit.Fact]
        public void Ctor_should_throw_ArgumentException_on_amount_less_than_zero()
        {
            Executing.This(() => new PositiveMoney(-1, "EUR"))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("amount");
        }
    }
}

using System;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using Moq;
using SharpTestsEx;
using Xunit;

namespace Merp.Accountancy.CommandStack.Tests.Model
{
    public class JobOrderFixture
    {
        public class Factory
        {
            [Fact]
            public void CreateNewInstance_should_throw_ArgumentNullException_on_null_jobOrderNumberGenerator()
            {
                Executing.This(() => JobOrder.Factory.CreateNewInstance(null, Guid.NewGuid(), string.Empty, Guid.NewGuid(), 101, "GBP", DateTime.Now, DateTime.Now.AddMonths(1), true, "A job order", null, "Description"))
                    .Should()
                    .Throw<ArgumentNullException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("jobOrderNumberGenerator");
            }

            [Fact]
            public void CreateNewInstance_should_throw_ArgumentException_on_price_lower_than_zero()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => JobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), string.Empty, Guid.NewGuid(), -1, "GBP", DateTime.Now, DateTime.Now.AddMonths(1), true, "A job order", null, "Description"))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("price");
            }

            [Fact]
            public void CreateNewInstance_should_throw_ArgumentException_on_null_currency()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => JobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), string.Empty, Guid.NewGuid(), 101, null, DateTime.Now, DateTime.Now.AddMonths(1), true, "A job order", null, "Description"))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("currency");
            }

            [Fact]
            public void CreateNewInstance_should_throw_ArgumentException_on_blank_currency()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => JobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), string.Empty, Guid.NewGuid(), 101, string.Empty, DateTime.Now, DateTime.Now.AddMonths(1), true, "A job order", null, "Description"))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("currency");
            }

            [Fact]
            public void CreateNewInstance_should_throw_ArgumentException_on_a_dueDate_preceding_the_startingDate()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => JobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), string.Empty, Guid.NewGuid(), 101, "GBP", DateTime.Now.AddMonths(1), DateTime.Now, true, "A job order", null, "Description"))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("dueDate");
            }

            [Fact]
            public void CreateNewInstance_should_throw_ArgumentException_on_null_name()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => JobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), string.Empty, Guid.NewGuid(), 101, "GBP", DateTime.Now, DateTime.Now.AddMonths(1), true, null, null, "Description"))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("name");
            }

            [Fact]
            public void CreateNewInstance_should_throw_ArgumentException_on_blank_name()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => JobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), string.Empty, Guid.NewGuid(), 101, "GBP", DateTime.Now, DateTime.Now.AddMonths(1), true, string.Empty, null, "Description"))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("name");
            }
        }

        //[TestFixture]
        //public class MarkAsCompleted_Method
        //{
        //      [Fact]
        //      public void Should_Throw_InvalidOperationException_On()
        //      {
        //      }
        //}

        //[TestFixture]
        //public class AssociateOutgoingInvoice_Method
        //{

        //}
    }
}

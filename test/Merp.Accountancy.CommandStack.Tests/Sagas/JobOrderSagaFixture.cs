﻿using System;
using Moq;
using NUnit.Framework;
using SharpTestsEx;
using Merp.Accountancy.CommandStack.Sagas;
using MementoFX.Persistence;
using Rebus.Bus;
using Xunit;

namespace Merp.Accountancy.CommandStack.Tests.Sagas
{
    public class JobOrderSagaFixture
    {
        [Fact]
        public void Ctor_should_throw_ArgumentNullException_on_null_jobOrderNumberGenerator()
        {
            var bus = new Mock<IBus>().Object;
            var eventStore = new Mock<IEventStore>().Object;
            var repository = new Mock<IRepository>().Object;

            Executing.This(() => new JobOrderSaga(bus, eventStore, repository, null))
                .Should()
                .Throw<ArgumentNullException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("jobOrderNumberGenerator");
        }
    }
}

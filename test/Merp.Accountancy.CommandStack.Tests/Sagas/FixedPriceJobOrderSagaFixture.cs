using System;
using Moq;
using NUnit.Framework;
using SharpTestsEx;
using Merp.Accountancy.CommandStack.Sagas;
using Memento.Persistence;
using Rebus.Bus;

namespace Merp.Accountancy.CommandStack.Tests.Sagas
{
    [TestFixture]
    public class FixedPriceJobOrderSagaFixture
    {
        //[Test]
        //public void Ctor_should_throw_ArgumentNullException_on_null_jobOrderNumberGenerator()
        //{
        //    var bus = new Mock<IBus>().Object;
        //    var eventStore = new Mock<IEventStore>().Object;
        //    var repository = new Mock<IRepository>().Object;

        //    Executing.This(() => new FixedPriceJobOrderSaga(bus, eventStore, repository, null))
        //        .Should()
        //        .Throw<ArgumentNullException>()
        //        .And
        //        .ValueOf
        //        .ParamName
        //        .Should()
        //        .Be
        //        .EqualTo("jobOrderNumberGenerator");
        //}
    }
}

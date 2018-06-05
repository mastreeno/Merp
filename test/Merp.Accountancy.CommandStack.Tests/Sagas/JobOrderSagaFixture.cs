using System;
using Moq;
using Xunit;
using SharpTestsEx;
using Merp.Accountancy.CommandStack.Sagas;
using MementoFX.Persistence;
using Rebus.Bus;
using Merp.Accountancy.CommandStack.Services;

namespace Merp.Accountancy.CommandStack.Tests.Sagas
{
    
    public class JobOrderSagaFixture
    {
        public class Constuctor
        {
            [Fact]
            public void Should_throw_ArgumentNullException_on_null_jobOrderNumberGenerator()
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

            [Fact]
            public void Should_set_JobOrderNumberGenerator_property()
            {
                var bus = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                var repository = new Mock<IRepository>().Object;
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;
                var sut = new JobOrderSaga(bus, eventStore, repository, jobOrderNumberGenerator);
                Assert.Equal(jobOrderNumberGenerator, sut.JobOrderNumberGenerator);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_on_null_eventStore()
            {
                var bus = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                var repository = new Mock<IRepository>().Object;
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => new JobOrderSaga(bus, null, repository, jobOrderNumberGenerator))
                    .Should()
                    .Throw<ArgumentNullException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("eventStore");
            }

            [Fact]
            public void Should_set_eventStore_property()
            {
                var bus = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                var repository = new Mock<IRepository>().Object;
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;
                var sut = new JobOrderSaga(bus, eventStore, repository, jobOrderNumberGenerator);
                Assert.Equal(eventStore, sut.eventStore);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_on_null_repository()
            {
                var bus = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                var repository = new Mock<IRepository>().Object;
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => new JobOrderSaga(bus, eventStore, null, jobOrderNumberGenerator))
                    .Should()
                    .Throw<ArgumentNullException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("repository");
            }

            [Fact]
            public void Should_set_repository_property()
            {
                var bus = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                var repository = new Mock<IRepository>().Object;
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;
                var sut = new JobOrderSaga(bus, eventStore, repository, jobOrderNumberGenerator);
                Assert.Equal(repository, sut.repository);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_on_null_bus()
            {
                var bus = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                var repository = new Mock<IRepository>().Object;
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => new JobOrderSaga(null, eventStore, repository, jobOrderNumberGenerator))
                    .Should()
                    .Throw<ArgumentNullException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("bus");
            }

            [Fact]
            public void Should_set_bus_property()
            {
                var bus = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                var repository = new Mock<IRepository>().Object;
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;
                var sut = new JobOrderSaga(bus, eventStore, repository, jobOrderNumberGenerator);
                Assert.Equal(bus, sut.bus);
            }
        }

    }
}

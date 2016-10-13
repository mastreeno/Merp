using System;
using NUnit.Framework;
using Moq;
using SharpTestsEx;
using Memento.Persistence;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.CommandStack.Services;
using Merp.Accountancy.CommandStack.Events;

namespace Merp.Accountancy.CommandStack.Tests.Model
{
    [TestFixture]
    public class TimeAndMaterialJobOrderFixture
    {
        [TestFixture]
        public class Factory
        {
            [Test]
            public void CreateNewInstance_should_throw_ArgumentNullException_on_null_jobOrderNumberGenerator()
            {
                Executing.This(() => TimeAndMaterialJobOrder.Factory.CreateNewInstance(null, Guid.NewGuid(), Guid.NewGuid(), 100, "GBP", DateTime.Now, DateTime.Now.AddMonths(1), "A job order", null, "Description"))
                    .Should()
                    .Throw<ArgumentNullException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("jobOrderNumberGenerator");
            }

            [Test]
            public void CreateNewInstance_should_throw_ArgumentException_on_value_lower_than_zero()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => TimeAndMaterialJobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), Guid.NewGuid(), -1, "GBP", DateTime.Now, DateTime.Now.AddMonths(1), "A job order", null, "Description"))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("value");
            }

            [Test]
            public void CreateNewInstance_should_throw_ArgumentException_on_null_currency()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => TimeAndMaterialJobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), Guid.NewGuid(), 101, null, DateTime.Now, DateTime.Now.AddMonths(1), "A job order", null, "Description"))
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
            public void CreateNewInstance_should_throw_ArgumentException_on_blank_currency()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => TimeAndMaterialJobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), Guid.NewGuid(), 101, string.Empty, DateTime.Now, DateTime.Now.AddMonths(1), "A job order", null, "Description"))
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
            public void CreateNewInstance_should_throw_ArgumentException_on_a_dateOfExpiration_preceding_the_startingDate()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => TimeAndMaterialJobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), Guid.NewGuid(), 101, "GBP", DateTime.Now.AddMonths(1), DateTime.Now, "A job order", null, "Description"))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("dateOfExpiration");
            }

            [Test]
            public void CreateNewInstance_should_throw_ArgumentException_on_null_name()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => TimeAndMaterialJobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), Guid.NewGuid(), 101, "GBP", DateTime.Now, DateTime.Now.AddMonths(1), null, null, "Description"))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("name");
            }

            [Test]
            public void CreateNewInstance_should_throw_ArgumentException_on_blank_name()
            {
                var jobOrderNumberGenerator = new Mock<IJobOrderNumberGenerator>().Object;

                Executing.This(() => TimeAndMaterialJobOrder.Factory.CreateNewInstance(jobOrderNumberGenerator, Guid.NewGuid(), Guid.NewGuid(), 101, "GBP", DateTime.Now, DateTime.Now.AddMonths(1), string.Empty, null, "Description"))
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

        [TestFixture]
        public class CalculateBalance_Method
        {
            [Test]
            public void CalculateBalance_With_IncomingInvoicesOnly()
            {
                var generator = new Mock<IJobOrderNumberGenerator>();
                generator
                    .Setup(o => o.Generate())
                    .Returns("101/1989");

                var jobOrderId = Guid.NewGuid();
                var incomingInvoiceId = Guid.NewGuid();
                var eventStore = new Mock<IEventStore>();
                eventStore
                    .Setup(o => o.Find(It.IsAny<Func<IncomingInvoiceLinkedToJobOrderEvent, bool>>()))
                    .Returns(new IncomingInvoiceLinkedToJobOrderEvent[] { new IncomingInvoiceLinkedToJobOrderEvent(incomingInvoiceId, jobOrderId, DateTime.Now, 100) });
                eventStore
                    .Setup(o => o.Find(It.IsAny<Func<IncomingInvoiceRegisteredEvent, bool>>()))
                    .Returns(new IncomingInvoiceRegisteredEvent[] { new IncomingInvoiceRegisteredEvent(incomingInvoiceId, "42", DateTime.Now, 100, 22, 122, "fake", "fake", "", Guid.NewGuid(), "", "", "", "", "", "", "") });

                decimal balance = JobOrder.CalculateBalance(eventStore.Object, jobOrderId);
                decimal expected = -100;
                Assert.AreEqual(expected, balance);
            }

            [Test]
            public void CalculateBalance_With_OutgoingInvoicesOnly()
            {
                var generator = new Mock<IJobOrderNumberGenerator>();
                generator
                    .Setup(o => o.Generate())
                    .Returns("101/1989");

                var jobOrderId = Guid.NewGuid();
                var outgoingInvoiceId = Guid.NewGuid();
                var eventStore = new Mock<IEventStore>();
                eventStore
                    .Setup(o => o.Find(It.IsAny<Func<OutgoingInvoiceLinkedToJobOrderEvent, bool>>()))
                    .Returns(new OutgoingInvoiceLinkedToJobOrderEvent[] { new OutgoingInvoiceLinkedToJobOrderEvent(outgoingInvoiceId, jobOrderId, DateTime.Now, 100) });
                eventStore
                    .Setup(o => o.Find(It.IsAny<Func<OutgoingInvoiceIssuedEvent, bool>>()))
                    .Returns(new OutgoingInvoiceIssuedEvent[] { new OutgoingInvoiceIssuedEvent(outgoingInvoiceId, "42", DateTime.Now, 100, 22, 122, "fake", "fake", "", Guid.NewGuid(), "", "", "", "", "", "", "") });

                decimal balance = JobOrder.CalculateBalance(eventStore.Object, jobOrderId);
                decimal expected = 100;
                Assert.AreEqual(expected, balance);
            }

            [Test]
            public void CalculateBalance_having_both_Incoming_and_Outgoing_Invoices()
            {
                var generator = new Mock<IJobOrderNumberGenerator>();
                generator
                    .Setup(o => o.Generate())
                    .Returns("101/1989");

                var eventStore = new Mock<IEventStore>();

                var jobOrderId = Guid.NewGuid();
                var outgoingInvoiceId = Guid.NewGuid();
                eventStore
                    .Setup(o => o.Find(It.IsAny<Func<OutgoingInvoiceLinkedToJobOrderEvent, bool>>()))
                    .Returns(new OutgoingInvoiceLinkedToJobOrderEvent[] { new OutgoingInvoiceLinkedToJobOrderEvent(outgoingInvoiceId, jobOrderId, DateTime.Now, 200) });
                eventStore
                    .Setup(o => o.Find(It.IsAny<Func<OutgoingInvoiceIssuedEvent, bool>>()))
                    .Returns(new OutgoingInvoiceIssuedEvent[] { new OutgoingInvoiceIssuedEvent(outgoingInvoiceId, "42", DateTime.Now, 200, 44, 244, "fake", "fake", "", Guid.NewGuid(), "", "", "", "", "", "", "") });

                var incomingInvoiceId = Guid.NewGuid();
                eventStore
                    .Setup(o => o.Find(It.IsAny<Func<IncomingInvoiceLinkedToJobOrderEvent, bool>>()))
                    .Returns(new IncomingInvoiceLinkedToJobOrderEvent[] { new IncomingInvoiceLinkedToJobOrderEvent(incomingInvoiceId, jobOrderId, DateTime.Now, 100) });
                eventStore
                    .Setup(o => o.Find(It.IsAny<Func<IncomingInvoiceRegisteredEvent, bool>>()))
                    .Returns(new IncomingInvoiceRegisteredEvent[] { new IncomingInvoiceRegisteredEvent(incomingInvoiceId, "42", DateTime.Now, 100, 22, 122, "fake", "fake", "", Guid.NewGuid(), "", "", "", "", "", "", "") });

                
                decimal balance = JobOrder.CalculateBalance(eventStore.Object, jobOrderId);
                decimal expected = 100;
                Assert.AreEqual(expected, balance);
            }
        }

        [TestFixture]
        public class MarkAsCompleted_Method
        {
            //[Test]
            //public void Should_Throw_InvalidOperationException_On()
            //{
            //}
        }
    }
}

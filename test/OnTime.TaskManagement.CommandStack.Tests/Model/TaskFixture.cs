using Memento.Domain;
using NUnit.Framework;
using OnTime.TaskManagement.CommandStack.Events;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTask = OnTime.TaskManagement.CommandStack.Model.Task;

namespace OnTime.TaskManagement.CommandStack.Tests.Model
{
    [TestFixture]
    public class TaskFixture
    {
        [Test]
        public void Create_should_build_a_valid_instance()
        {
            var userID = Guid.NewGuid();
            var taskName = "Fake";
            var task = OTask.Factory.Create(userID, taskName);
            Assert.AreEqual(taskName, task.Name);
            Assert.AreEqual(userID, task.CreatorId);
            Assert.IsFalse(task.DateOfCancellation.HasValue);
            Assert.IsFalse(task.DateOfCompletion.HasValue);
            Assert.IsFalse(task.DueDate.HasValue);
            Assert.AreNotEqual(Guid.Empty, task.Id);
        }

        [Test]
        public void Create_should_throw_ArgumentException_on_empty_userId()
        {
            var userID = Guid.Empty;
            var taskName = "Fake";
            Executing.This(() => OTask.Factory.Create(userID, taskName))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("userId");
        }

        [Test]
        public void Create_should_throw_ArgumentException_on_empty_taskName()
        {
            var userID = Guid.NewGuid();
            var taskName = string.Empty;
            Executing.This(() => OTask.Factory.Create(userID, taskName))
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
        public void Create_should_throw_ArgumentException_on_null_taskName()
        {
            var userID = Guid.NewGuid();
            string taskName = null;
            Executing.This(() => OTask.Factory.Create(userID, taskName))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ValueOf
                .ParamName
                .Should()
                .Be
                .EqualTo("name");
        }

        [TestFixture]
        public class Behaviour
        {
            [Test]
            public void Cancel_should_throw_InvalidOperationException_for_already_cancelled_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Executing.This(() => task.Cancel(userId))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Test]
            public void Cancel_should_throw_InvalidOperationException_for_completed_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Executing.This(() => task.Cancel(userId))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Test]
            public void Cancel_should_set_DateOfCancellation()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Assert.IsTrue(task.DateOfCancellation.HasValue);
            }

            [Test]
            public void Cancel_should_throw_ArgumentException_if_the_provided_userId_is_not_the_one_of_the_task_creator()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                Executing.This(() => task.Cancel(Guid.NewGuid()))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("userId");         
            }

            [Test]
            public void MarkAsCompleted_should_throw_InvalidOperationException_for_already_completed_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Executing.This(() => task.MarkAsCompleted(userId))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Test]
            public void MarkAsCompleted_should_throw_InvalidOperationException_for_cancelled_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Executing.This(() => task.MarkAsCompleted(userId))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Test]
            public void MarkAsCompleted_should_set_DateOfCompletion()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Assert.IsTrue(task.DateOfCompletion.HasValue);
            }

            [Test]
            public void MarkAsCompleted_should_throw_ArgumentException_if_the_provided_userId_is_not_the_one_of_the_task_creator()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                Executing.This(() => task.MarkAsCompleted(Guid.NewGuid()))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("userId");
            }

            [Test]
            public void Reactivate_should_throw_InvalidOperationException_for_active_tasks()
            {
                var task = OTask.Factory.Create(Guid.NewGuid(), "Fake");
                Executing.This(() => task.Reactivate())
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Test]
            public void Reactivate_should_set_DateOfCompletion_to_null_for_completed_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Assert.IsTrue(task.DateOfCompletion.HasValue);
                task.Reactivate();
                Assert.IsFalse(task.DateOfCompletion.HasValue);
            }

            [Test]
            public void Reactivate_should_set_DateOfCompletion_to_null_for_cancelled_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Assert.IsTrue(task.DateOfCancellation.HasValue);
                task.Reactivate();
                Assert.IsFalse(task.DateOfCancellation.HasValue);
            }

            [Test]
            public void Rename_should_throw_InvalidOperationException_for_completed_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Executing.This(() => task.Rename("Let's have a Black Celebration tonight"))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Test]
            public void Rename_should_throw_InvalidOperationException_for_cancelled_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Executing.This(() => task.Rename("Let's have a Black Celebration tonight"))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Test]
            public void Rename_should_throw_ArgumentException_on_null_proposedName()
            {
                var task = OTask.Factory.Create(Guid.NewGuid(), "Fake");
                Executing.This(() => task.Rename(null))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("proposedName");
            }

            [Test]
            public void Rename_should_throw_ArgumentException_on_empty_proposedName()
            {
                var task = OTask.Factory.Create(Guid.NewGuid(), "Fake");
                Executing.This(() => task.Rename(""))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("proposedName");
            }

            [Test]
            public void Rename_should_throw_ArgumentException_on_whitespace_proposedName()
            {
                var task = OTask.Factory.Create(Guid.NewGuid(), "Fake");
                Executing.This(() => task.Rename(" "))
                    .Should()
                    .Throw<ArgumentException>()
                    .And
                    .ValueOf
                    .ParamName
                    .Should()
                    .Be
                    .EqualTo("proposedName");
            }

            [Test]
            public void Rename_should_change_Name_property_accordingly()
            {
                var task = OTask.Factory.Create(Guid.NewGuid(), "Fake");
                var proposedName = "Buy records";
                task.Rename(proposedName);
                Assert.AreEqual(proposedName, task.Name);
            }
        }

        [TestFixture]
        public class ApplyEvent
        {
            [Test]
            public void TaskCompletedEvent()
            {
                var taskId = Guid.NewGuid();
                var e = new TaskCompletedEvent()
                {
                    DateOfCompletion = DateTime.Now,
                    TaskId = taskId
                };
                var task = OTask.Factory.Create(taskId, "Fake");
                task.ApplyEvent(e);
                Assert.AreEqual(e.DateOfCompletion, task.DateOfCompletion);
            }

            [Test]
            public void TaskCancelledEvent()
            {
                var taskId = Guid.NewGuid();
                var e = new TaskCancelledEvent()
                {
                    DateOfCancellation = DateTime.Now,
                    TaskId = taskId
                };
                var task = OTask.Factory.Create(taskId, "Fake");
                task.ApplyEvent(e);
                Assert.AreEqual(e.DateOfCancellation, task.DateOfCancellation);
            }

            [Test]
            public void TaskRenamedEvent()
            {
                var taskId = Guid.NewGuid();
                var e = new TaskRenamedEvent()
                {
                    TaskName = "Brand new name",
                    TaskId = taskId
                };
                var task = OTask.Factory.Create(taskId, "Fake");
                task.ApplyEvent(e);
                Assert.AreEqual(e.TaskName, task.Name);
            }


            [Test]
            public void TaskReactivatedEvent()
            {
                var taskId = Guid.NewGuid();
                var e = new TaskReactivatedEvent()
                {
                    TaskId = taskId
                };
                var task = OTask.Factory.Create(taskId, "Fake");
                task.ApplyEvent(e);
                Assert.IsNull(task.DateOfCancellation);
                Assert.IsNull(task.DateOfCompletion);
            }
        }
    }
}

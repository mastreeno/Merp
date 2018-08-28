using MementoFX.Domain;
using Xunit;
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
    
    public class TaskFixture
    {
        [Fact]
        public void Create_should_build_a_valid_instance()
        {
            var userID = Guid.NewGuid();
            var taskName = "Fake";
            var task = OTask.Factory.Create(userID, taskName);
            Assert.Equal(taskName, task.Name);
            Assert.Equal(userID, task.CreatorId);
            Assert.False(task.DateOfCancellation.HasValue);
            Assert.False(task.DateOfCompletion.HasValue);
            Assert.False(task.DueDate.HasValue);
            Assert.NotEqual(Guid.Empty, task.Id);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        
        public class Behaviour
        {
            [Fact]
            public void Cancel_should_throw_InvalidOperationException_for_already_cancelled_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Executing.This(() => task.Cancel(userId))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Fact]
            public void Cancel_should_throw_InvalidOperationException_for_completed_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Executing.This(() => task.Cancel(userId))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Fact]
            public void Cancel_should_set_DateOfCancellation()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Assert.True(task.DateOfCancellation.HasValue);
            }

            [Fact]
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

            [Fact]
            public void MarkAsCompleted_should_throw_InvalidOperationException_for_already_completed_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Executing.This(() => task.MarkAsCompleted(userId))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Fact]
            public void MarkAsCompleted_should_throw_InvalidOperationException_for_cancelled_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Executing.This(() => task.MarkAsCompleted(userId))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Fact]
            public void MarkAsCompleted_should_set_DateOfCompletion()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Assert.True(task.DateOfCompletion.HasValue);
            }

            [Fact]
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

            [Fact]
            public void Reactivate_should_throw_InvalidOperationException_for_active_tasks()
            {
                var task = OTask.Factory.Create(Guid.NewGuid(), "Fake");
                Executing.This(() => task.Reactivate())
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Fact]
            public void Reactivate_should_set_DateOfCompletion_to_null_for_completed_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Assert.True(task.DateOfCompletion.HasValue);
                task.Reactivate();
                Assert.False(task.DateOfCompletion.HasValue);
            }

            [Fact]
            public void Reactivate_should_set_DateOfCompletion_to_null_for_cancelled_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Assert.True(task.DateOfCancellation.HasValue);
                task.Reactivate();
                Assert.False(task.DateOfCancellation.HasValue);
            }

            [Fact]
            public void Rename_should_throw_InvalidOperationException_for_completed_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.MarkAsCompleted(userId);
                Executing.This(() => task.Rename("Let's have a Black Celebration tonight"))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Fact]
            public void Rename_should_throw_InvalidOperationException_for_cancelled_tasks()
            {
                var userId = Guid.NewGuid();
                var task = OTask.Factory.Create(userId, "Fake");
                task.Cancel(userId);
                Executing.This(() => task.Rename("Let's have a Black Celebration tonight"))
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Fact]
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

            [Fact]
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

            [Fact]
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

            [Fact]
            public void Rename_should_change_Name_property_accordingly()
            {
                var task = OTask.Factory.Create(Guid.NewGuid(), "Fake");
                var proposedName = "Buy records";
                task.Rename(proposedName);
                Assert.Equal(proposedName, task.Name);
            }
        }

        
        public class ApplyEvent
        {
            [Fact]
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
                Assert.Equal(e.DateOfCompletion, task.DateOfCompletion);
            }

            [Fact]
            public void TaskCancelledEvent()
            {
                var taskId = Guid.NewGuid();
                var e = new TaskDeletedEvent()
                {
                    DateOfCancellation = DateTime.Now,
                    TaskId = taskId
                };
                var task = OTask.Factory.Create(taskId, "Fake");
                task.ApplyEvent(e);
                Assert.Equal(e.DateOfCancellation, task.DateOfCancellation);
            }

            [Fact]
            public void TaskRenamedEvent()
            {
                var taskId = Guid.NewGuid();
                var e = new TaskUpdatedEvent()
                {
                    Text = "Brand new name",
                    TaskId = taskId
                };
                var task = OTask.Factory.Create(taskId, "Fake");
                task.ApplyEvent(e);
                Assert.Equal(e.Text, task.Name);
            }


            [Fact]
            public void TaskReactivatedEvent()
            {
                var taskId = Guid.NewGuid();
                var e = new TaskReactivatedEvent()
                {
                    TaskId = taskId
                };
                var task = OTask.Factory.Create(taskId, "Fake");
                task.ApplyEvent(e);
                Assert.Null(task.DateOfCancellation);
                Assert.Null(task.DateOfCompletion);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTask = OnTime.TaskManagement.QueryStack.Model.Task;

namespace OnTime.TaskManagement.QueryStack.Model.Extensions
{
    public static class TaskExtensions
    {
        public static IQueryable<OTask> Current(this IQueryable<OTask> set)
        {
            return set.Where(t => !t.IsCompleted);
        }

        public static IQueryable<OTask> WithDueDateOn(this IQueryable<OTask> set, DateTime dateTime)
        {
            return set.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dateTime.Date);
        }

        public static IQueryable<OTask> WithDateOfCompletion(this IQueryable<OTask> set)
        {
            return set.Where(t => t.IsCompleted && t.DateOfCompletion.HasValue);
        }

        public static IQueryable<OTask> OfUser(this IQueryable<OTask> set, Guid userId)
        {
            return set.Where(t => t.UserId == userId);
        }

        public static IQueryable<OTask> Backlog(this IQueryable<OTask> set, Guid userId)
        {
            return set.OfUser(userId)
                        .Current()
                        .OrderBy(t => t.DateOfCreation);
        }

        public static IQueryable<OTask> Today(this IQueryable<OTask> set, Guid userId)
        {
            return set.OfUser(userId)
                        .WithDueDateOn(DateTime.Now)
                        .OrderBy(t => t.DateOfCreation);
        }

        public static IQueryable<OTask> ThisWeek(this IQueryable<OTask> set, Guid userId, DateTime endOfWeek)
        {
            return set.OfUser(userId)
                        .WithDueDateOn(endOfWeek)
                        .OrderBy(t => t.DateOfCreation);
        }

        public static IQueryable<OTask> Completed(this IQueryable<OTask> set, Guid userId)
        {
            return set.OfUser(userId)
                        .WithDateOfCompletion()
                        .OrderBy(t => t.DateOfCreation);
        }
    }
}

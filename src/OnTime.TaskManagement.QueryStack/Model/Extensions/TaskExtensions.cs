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
        public static IQueryable<OTask> Backlog(this IQueryable<OTask> set, Guid userId)
        {
            return set.OfUser(userId)
                        .Current(userId)
                        .OrderBy(t => t.DateOfCreation);
        }

        public static IQueryable<OTask> Today(this IQueryable<OTask> set, Guid userId)
        {
            var today = DateTime.Now.Date;
            return set.OfUser(userId)
                        .Current(userId)
                        .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == today)
                        .OrderBy(t => t.DateOfCreation);
        }

        private static IQueryable<OTask> OfUser(this IQueryable<OTask> set, Guid userId)
        {
            return set.Where(t => t.UserId == userId);
        }

        private static IQueryable<OTask> Current(this IQueryable<OTask> set, Guid userId)
        {
            return set.OfUser(userId)
                        .Where(t => !t.IsCompleted);
        }
    }
}

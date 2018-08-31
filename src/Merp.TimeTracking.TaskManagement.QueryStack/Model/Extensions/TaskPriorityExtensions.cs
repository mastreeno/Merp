using System;
using System.Collections.Generic;
using System.Text;
using DomainTaskPriority = Merp.TimeTracking.TaskManagement.CommandStack.Model.TaskPriority;
using ReadModelTaskPriority = Merp.TimeTracking.TaskManagement.QueryStack.Model.TaskPriority;

namespace Merp.TimeTracking.TaskManagement.QueryStack.Model.Extensions
{
    public static class TaskPriorityExtensions
    {
        public static ReadModelTaskPriority Convert(this DomainTaskPriority priority)
        {
            switch(priority)
            {
                case DomainTaskPriority.Low:
                    return ReadModelTaskPriority.Low;
                case DomainTaskPriority.Standard:
                    return ReadModelTaskPriority.Standard;
                case DomainTaskPriority.High:
                    return ReadModelTaskPriority.High;
                case DomainTaskPriority.Critical:
                    return ReadModelTaskPriority.Critical;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static DomainTaskPriority Convert(this ReadModelTaskPriority priority)
        {
            switch (priority)
            {
                case ReadModelTaskPriority.Low:
                    return DomainTaskPriority.Low;
                case ReadModelTaskPriority.Standard:
                    return DomainTaskPriority.Standard;
                case ReadModelTaskPriority.High:
                    return DomainTaskPriority.High;
                case ReadModelTaskPriority.Critical:
                    return DomainTaskPriority.Critical;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

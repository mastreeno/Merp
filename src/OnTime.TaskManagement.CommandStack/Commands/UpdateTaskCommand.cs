using MementoFX;
using OnTime.TaskManagement.CommandStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTime.TaskManagement.CommandStack.Commands
{
    public class UpdateTaskCommand : Command
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public Guid? JobOrderId { get; set; }
        public TaskPriority Priority { get; set; }

        public UpdateTaskCommand(Guid taskId, Guid userId, string name, TaskPriority priority)
        {
            if (taskId == Guid.Empty)
                throw new ArgumentException("taskId can't be empty", nameof(taskId));
            if (userId == Guid.Empty)
                throw new ArgumentException("userId can't be empty", nameof(userId));
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name can't be empty", nameof(name));

            TaskId = taskId;
            UserId = userId;
            Name = name;
            Priority = priority;
        }
    }
}

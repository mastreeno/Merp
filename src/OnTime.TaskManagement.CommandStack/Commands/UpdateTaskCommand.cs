using MementoFX;
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
        public Guid? JobOrderId { get; set; }
        public string Text { get; set; }

        public UpdateTaskCommand(Guid taskId, Guid userId, string text)
        {
            if (taskId == Guid.Empty)
                throw new ArgumentException("taskId can't be empty", nameof(taskId));
            if (userId == Guid.Empty)
                throw new ArgumentException("userId can't be empty", nameof(userId));
            if(string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("text can't be empty", nameof(text));

            TaskId = taskId;
            UserId = userId;
            Text = text;
        }
    }
}

using Merp.TimeTracking.TaskManagement.QueryStack.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.TimeTracking.Web.Areas.TaskManagement.Models.Task
{
    public class UpdateModel
    {
        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public string Name { get; set; }

        public TaskPriority Priority { get; set; }

        public Guid? JobOrderId { get; set; }
    }
}

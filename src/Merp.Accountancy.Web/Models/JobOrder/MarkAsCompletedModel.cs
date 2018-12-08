using System;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models.JobOrder
{
    public class MarkAsCompletedModel
    {
        [Required]
        public DateTime DateOfCompletion { get; set; }
    }
}

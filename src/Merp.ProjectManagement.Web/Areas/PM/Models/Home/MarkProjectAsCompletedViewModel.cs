using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Merp.Web.Site.Areas.ProjectManagement.Models.Home
{
    public class MarkProjectAsCompletedViewModel
    {
        [Required]
        public Guid ProjectId { get; set; }

        [DisplayName("Project #")]
        public string ProjectNumber { get; set; }

        [DisplayName("Project name")]
        public string ProjectName { get; set; }

        [DisplayName("Customer name")]
        public string CustomerName { get; set; }

        [DisplayName("Date of completion")]
        [Required]
        public DateTime DateOfCompletion { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OnTime.Web.Site.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(40)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }
    }
}

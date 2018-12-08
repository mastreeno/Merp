using System.ComponentModel.DataAnnotations;

namespace Merp.Web.Auth.Models.Manage
{
    public class ProfileViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Models
{
    public class PersonInfo
    {
        [Required]
        public int Id { get; set; }
        public Guid OriginalId { get; set; }
        public string Name { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.Models
{
    public class PartyInfo
    {
        [Required]
        public int Id { get; set; }
        public Guid OriginalId { get; set; }
        public string Name { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Merp.Registry.Web.Api.Internal.Models.Party
{
    public class PartyInfoModel
    {
        [Required]
        public int Id { get; set; }
        public Guid OriginalId { get; set; }
        public string Name { get; set; }
    }
}

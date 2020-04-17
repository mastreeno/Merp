using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merp.Registry.QueryStack.Model
{
    public class Company : Party
    {
        [Required]
        public string CompanyName { get; set; }

        public Guid MainContactUid { get; set; }
        public string MainContactName { get; set; }

        public Guid AdministrativeContactUid { get; set; }
        public string AdministrativeContactName { get; set; }
    }
}

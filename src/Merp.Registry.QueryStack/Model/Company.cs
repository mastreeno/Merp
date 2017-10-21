using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merp.Registry.QueryStack.Model
{
    public class Company : IEntityTypeConfiguration<Company>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        //[Index]
        public Guid OriginalId { get; set; }

        public string VatIndex { get; set; }

        public string NationalIdentificationNumber { get; set; }

        [Required]
        public string CompanyName { get; set; }
        public string MainContact { get; set; }
        public string AdministrativeContact { get; set; }

        void IEntityTypeConfiguration<Company>.Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasIndex(o => o.OriginalId);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merp.Registry.QueryStack.Model
{
    public class Person : IEntityTypeConfiguration<Person>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        //[Index]
        public Guid OriginalId { get; set; }

        //[Index]
        [MaxLength(200)]
        public string DisplayName { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string VatIndex { get; set; }

        public string NationalIdentificationNumber { get; set; }

        void IEntityTypeConfiguration<Person>.Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasIndex(o => o.OriginalId);
            builder.HasIndex(o => o.DisplayName);
        }

    }
}

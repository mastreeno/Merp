using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Merp.Registry.QueryStack.Model
{
    public class Party : IEntityTypeConfiguration<Party>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        //[Index]
        public Guid OriginalId { get; set; }
        [Required]
        public PartyType Type { get; set; }

        //[Index]
        [MaxLength(200)]
        public string DisplayName { get; set; }

        public string VatIndex { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public PostalAddress LegalAddress { get; set; }

        //public ContactInfo ContactInfo { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }
        public string InstantMessaging { get; set; }

        public enum PartyType
        {
            Company,
            Person
        }

        void IEntityTypeConfiguration<Party>.Configure(EntityTypeBuilder<Party> builder)
        {
            builder.HasIndex(o => o.OriginalId);
            builder.HasIndex(o => o.DisplayName);
            builder.OwnsOne(o => o.LegalAddress);
            //builder.OwnsOne(o => o.ContactInfo);
        }
    }
}

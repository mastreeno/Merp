using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Merp.Registry.QueryStack.Model
{
    public class Party
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public Guid OriginalId { get; set; }

        [Required]
        public PartyType Type { get; set; }

        [MaxLength(200)]
        public string DisplayName { get; set; }
        public string VatIndex { get; set; }
        public string NationalIdentificationNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }
        public string InstantMessaging { get; set; }
        public bool Unlisted { get; set; }
        //public ContactInfo ContactInfo { get; set; }
        public PostalAddress BillingAddress { get; set; }
        public PostalAddress LegalAddress { get; set; }
        public PostalAddress ShippingAddress { get; set; }


        public enum PartyType : int
        {
            Company = 0,
            Person = 1,
            Unspecified = 2
        }
    }
}

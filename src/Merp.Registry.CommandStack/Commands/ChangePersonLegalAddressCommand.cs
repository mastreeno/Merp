using System;
using Merp.Domain;

namespace Merp.Registry.CommandStack.Commands
{
    public class ChangePersonLegalAddressCommand : MerpCommand
    {
        public Guid PersonId { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public DateTime EffectiveDate { get; set; }

        public ChangePersonLegalAddressCommand(Guid userId, Guid personId, string address, string postalCode, string city, string province, string country, DateTime effectiveDate)
            : base(userId)
        {
            PersonId = personId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            PostalCode = postalCode;
            City = city ?? throw new ArgumentNullException(nameof(city));
            Country = country;
            Province = province;
            EffectiveDate = effectiveDate;
        }
    }
}

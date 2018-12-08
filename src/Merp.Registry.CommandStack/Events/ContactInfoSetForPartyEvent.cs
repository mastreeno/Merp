using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Events
{
    public class ContactInfoSetForPartyEvent : MerpDomainEvent
    {
        public Guid PartyId { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }
        public string InstantMessaging { get; set; }

        public ContactInfoSetForPartyEvent(Guid partyId, string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging, Guid userId)
            : base(userId)
        {
            PartyId = partyId;
            PhoneNumber = phoneNumber;
            MobileNumber = mobileNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
            InstantMessaging = instantMessaging;
        }
    }
}

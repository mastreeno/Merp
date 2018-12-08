using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class ChangePersonContactInfoCommand : MerpCommand
    {
        public Guid PersonId { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }
        public string InstantMessaging { get; set; }

        public ChangePersonContactInfoCommand(Guid userId, Guid personId, string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging)
            : base(userId)
        {
            PersonId = personId;
            PhoneNumber = phoneNumber;
            MobileNumber = mobileNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
            InstantMessaging = instantMessaging;
        }
    }
}

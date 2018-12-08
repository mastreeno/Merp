using Merp.Domain;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class ChangeCompanyContactInfoCommand : MerpCommand
    {
        public Guid CompanyId { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }

        public ChangeCompanyContactInfoCommand(Guid userId, Guid companyId, string phoneNumber, string faxNumber, string websiteAddress, string emailAddress)
            : base(userId)
        {
            CompanyId = companyId;
            PhoneNumber = phoneNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
        }
    }
}

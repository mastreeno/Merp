using MementoFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Commands
{
    public class ChangeCompanyContactInfoCommand : Command
    {
        public Guid CompanyId { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }

        public ChangeCompanyContactInfoCommand(Guid companyId, string phoneNumber, string faxNumber, string websiteAddress, string emailAddress)
        {
            CompanyId = companyId;
            PhoneNumber = phoneNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
        }
    }
}

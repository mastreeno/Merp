using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Model
{
    public class ContactInfo
    {
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string EmailAddress { get; set; }
        public string InstantMessaging { get; set; }
        public string Pec { get; set; }

        public ContactInfo(string phoneNumber, string mobileNumber, string faxNumber, string websiteAddress, string emailAddress, string instantMessaging, string pec)
        {
            PhoneNumber = phoneNumber;
            MobileNumber = mobileNumber;
            FaxNumber = faxNumber;
            WebsiteAddress = websiteAddress;
            EmailAddress = emailAddress;
            InstantMessaging = instantMessaging;
            Pec = pec;
        }
    }
}

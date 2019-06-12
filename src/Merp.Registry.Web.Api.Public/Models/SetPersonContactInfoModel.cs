using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Api.Public.Models
{
    public class SetPersonContactInfoModel
    {
        public string EmailAddress { get; set; }

        public string FaxNumber { get; set; }

        public string InstantMessaging { get; set; }

        public string MobileNumber { get; set; }

        public Guid PersonId { get; set; }

        public string PhoneNumber { get; set; }

        public Guid UserId { get; set; }

        public string WebsiteAddress { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Api.Public.Models
{
    public class AssociateCompanyMainContactModel
    {
        public Guid CompanyId { get; set; }

        public Guid MainContactId { get; set; }

        public Guid UserId { get; set; }
    }
}

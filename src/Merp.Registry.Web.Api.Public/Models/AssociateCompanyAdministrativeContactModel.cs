using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Registry.Web.Api.Public.Models
{
    public class AssociateCompanyAdministrativeContactModel
    {
        public Guid AdministrativeContactId { get; set; }

        public Guid CompanyId { get; set; }

        public Guid UserId { get; set; }
    }
}

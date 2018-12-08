using System.ComponentModel;

namespace Merp.Registry.Web.Models.Company
{
    public class AssociateAdministrativeContactModel
    {
        [DisplayName("Administrative Contact")]
        public PersonInfo AdministrativeContact { get; set; }
    }
}

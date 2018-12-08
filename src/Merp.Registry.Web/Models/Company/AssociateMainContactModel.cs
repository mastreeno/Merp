using System.ComponentModel;

namespace Merp.Registry.Web.Models.Company
{
    public class AssociateMainContactModel
    {
        [DisplayName("Administrative Contact")]
        public PersonInfo MainContact { get; set; }
    }
}

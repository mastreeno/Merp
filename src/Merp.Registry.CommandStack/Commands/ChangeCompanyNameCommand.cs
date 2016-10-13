using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento;
using Memento.Domain;

namespace Merp.Registry.CommandStack.Commands
{
    public class ChangeCompanyNameCommand : Command
    {
        public Guid CompanyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string CompanyName { get; set; }

        public ChangeCompanyNameCommand(Guid companyId, string companyName, DateTime effectiveDate)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            EffectiveDate = effectiveDate;
        }
    }
}

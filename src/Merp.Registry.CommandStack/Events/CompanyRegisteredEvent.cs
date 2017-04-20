using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Events
{
    public class CompanyRegisteredEvent : DomainEvent
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string VatIndex { get; set; }
        public string NationalIdentificationNumber { get; set; }

        public CompanyRegisteredEvent(Guid companyId, string companyName, string vatIndex, string nationalIdentificationNumber)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            VatIndex = vatIndex;
            NationalIdentificationNumber = nationalIdentificationNumber;
        }
    }
}

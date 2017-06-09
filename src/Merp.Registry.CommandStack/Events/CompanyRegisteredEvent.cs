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
        
        public string LegalAddressAddress { get; set; }
        public string LegalAddressCity { get; set; }
        public string LegalAddressPostalCode { get; set; }
        public string LegalAddressProvince { get; set; }
        public string LegalAddressCountry { get; set; }

        public CompanyRegisteredEvent(Guid companyId, string companyName, string vatIndex, string nationalIdentificationNumber, string legalAddressAddress, string legalAddressCity, string legalAddressPostalCode, string legalAddressProvince, string legalAddressCountry)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            VatIndex = vatIndex;
            NationalIdentificationNumber = nationalIdentificationNumber;

            LegalAddressAddress = legalAddressAddress;
            LegalAddressCity = legalAddressCity;
            LegalAddressPostalCode = legalAddressPostalCode;
            LegalAddressProvince = legalAddressProvince;
            LegalAddressCountry = legalAddressCountry;
        }
    }
}

using Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Commands
{
    /// <summary>
    /// The command which allows to import a person from an external source
    /// </summary>
    public class ImportCompanyCommand : Command
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
        public string NationalIdentificationNumber { get; set; }

        public ImportCompanyCommand(Guid companyId, string companyName, string vatNumber, string nationalIdentificationNumber)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            VatNumber = vatNumber;
            NationalIdentificationNumber = nationalIdentificationNumber;
        }

    }
}

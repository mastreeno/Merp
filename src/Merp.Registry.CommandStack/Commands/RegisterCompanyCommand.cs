using Memento;
using System;

namespace Merp.Registry.CommandStack.Commands
{
    public class RegisterCompanyCommand : Command
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string VatIndex { get; set; }

        public RegisterCompanyCommand(string companyName, string vatIndex)
        {
            CompanyName = companyName;
            VatIndex = vatIndex;
        }
    }
}

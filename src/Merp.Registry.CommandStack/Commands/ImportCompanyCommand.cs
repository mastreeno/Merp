using Memento;
using Merp.Registry.CommandStack.Helpers;
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
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

        public ImportCompanyCommand(Guid companyId, string companyName, string vatNumber, string nationalIdentificationNumber, string address, string city, string postalCode, string province, string country)
        {
            if (string.IsNullOrWhiteSpace(companyName))
            {
                throw new ArgumentNullException(nameof(companyName));
            }
            if (string.IsNullOrWhiteSpace(vatNumber))
            {
                throw new ArgumentNullException(nameof(vatNumber));
            }
            if (!PostalAddressHelper.IsValidAddress(address, city, postalCode, province, country))
            {
                throw new ArgumentException("postal address must either be empty or comprehensive of both address and city");
            }

            CompanyId = companyId;
            CompanyName = companyName;
            VatNumber = vatNumber;
            NationalIdentificationNumber = nationalIdentificationNumber;
            Address = address;
            City = city;
            PostalCode = postalCode;
            Province = province;
            Country = country;
        }

    }
}

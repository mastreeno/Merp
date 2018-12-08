using Acl.RegistryResolutionServices.Vies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acl.RegistryResolutionServices.Mappers
{
    internal class BGCompanyInformationMapper : CompanyInformationMapper
    {
        protected override void MapAddress(CompanyInformation companyInformation, string address)
        {
            var addressParts = address.Split(new[] { "\n" }, StringSplitOptions.None);
            companyInformation.Address = addressParts.Length > 0 ? addressParts[0] : null;
            companyInformation.City = addressParts.Length > 1 ? addressParts[1] : null;
        }
    }
}

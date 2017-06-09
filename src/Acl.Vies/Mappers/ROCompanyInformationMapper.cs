using Acl.Vies.Vies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acl.Vies.Mappers
{
    internal class ROCompanyInformationMapper : CompanyInformationMapper
    {
        protected override void MapAddress(CompanyInformation companyInformation, string address)
        {
            var addressParts = address.Split(new[] { "\n" }, StringSplitOptions.None);
            companyInformation.City = addressParts.Length > 0 ? addressParts[0] : null;
            companyInformation.Address = addressParts.Length > 1 ? addressParts[1] : null;            
        }
    }
}

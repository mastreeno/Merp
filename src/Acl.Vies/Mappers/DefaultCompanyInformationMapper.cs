using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acl.Vies.Mappers
{
    internal class DefaultCompanyInformationMapper : CompanyInformationMapper
    {
        protected override void MapAddress(CompanyInformation companyInformation, string address)
        {
            companyInformation.Address = address;
        }
    }
}

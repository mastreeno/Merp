using Acl.Vies.Vies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acl.Vies.Mappers
{
    internal class GBCompanyInformationMapper : CompanyInformationMapper
    {
        protected override void MapAddress(CompanyInformation companyInformation, string address)
        {
            var addressParts = address.Split(new[] { "\n" }, StringSplitOptions.None);

            switch (addressParts.Length)
            {
                case 5:
                    companyInformation.Address = string.Join(" ", addressParts[0], addressParts[1], addressParts[2]).Trim();
                    companyInformation.City = string.Join(" ", addressParts[3], addressParts[4]).Trim();
                    break;
                case 6:
                    companyInformation.Address = string.Join(" ", addressParts[0], addressParts[1], addressParts[2]).Trim();
                    companyInformation.City = addressParts[3];
                    companyInformation.Province = addressParts[4];
                    break;
                default:
                    companyInformation.Address = address;
                    break;
            }            
        }
    }
}

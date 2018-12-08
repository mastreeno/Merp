using Acl.RegistryResolutionServices.Vies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acl.RegistryResolutionServices.Mappers
{
    internal class BECompanyInformationMapper : CompanyInformationMapper
    {
        protected override void MapAddress(CompanyInformation companyInformation, string address)
        {
            var addressParts = address.Split(new[] { "\n" }, StringSplitOptions.None);
            companyInformation.Address = addressParts.Length > 0 ? addressParts[0] : null;
            companyInformation.City = addressParts.Length > 1 ? GetCity(addressParts[1]) : null;
            companyInformation.PostalCode = addressParts.Length > 1 ? GetPostalCode(addressParts[1]) : null;
        }



        private string GetPostalCode(string stringData)
        {
            return stringData.Split(' ').FirstOrDefault();
        }

        private string GetCity(string stringData)
        {
            var tokens = stringData.Split(' ');
            if (tokens.Length < 2)
            {
                return null;
            }
            var subset = tokens.Skip(1).Take(tokens.Length - 1).ToArray();
            return string.Join(" ", subset);
        }
    }
}

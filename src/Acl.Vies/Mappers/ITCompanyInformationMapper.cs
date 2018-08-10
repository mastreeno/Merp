using Acl.RegistryResolutionServices.Vies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acl.RegistryResolutionServices.Mappers
{
    internal class ITCompanyInformationMapper : CompanyInformationMapper
    {
        private const string NotResidingInItaly = "SOGGETTO IDENTIFICATO MA NON RESIDENTE IN ITALIA";

        protected override bool CanMapAddress(string address)
        {
            return !string.IsNullOrWhiteSpace(address) && address != NotResidingInItaly;
        }

        protected override void MapAddress(CompanyInformation companyInformation, string address)
        {
            var addressParts = address.Split(new[] { "\n" }, StringSplitOptions.None);

            companyInformation.Address = addressParts.Length > 0 ? addressParts[0] : null;

            if (addressParts.Length > 1)
            {
                companyInformation.City = GetCity(addressParts[1]);
                companyInformation.PostalCode = GetPostalCode(addressParts[1]);
                companyInformation.Province = GetProvince(addressParts[1]);
            }
        }

        private string GetProvince(string stringData)
        {
            return stringData.Split(' ').LastOrDefault();
        }

        private string GetPostalCode(string stringData)
        {
            return stringData.Split(' ').FirstOrDefault();
        }

        private string GetCity(string stringData)
        {
            var tokens = stringData.Split(' ');
            if(tokens.Length < 2)
            {
                return null;
            }
            var subset = tokens.Skip(1).Take(tokens.Length - 2).ToArray();
            return string.Join(" ", subset);
        }
    }
}

using System;
using System.Linq;

namespace Acl.RegistryResolutionServices.Mappers
{
    internal class ITPersonInformationMapper : PersonInformationMapper
    {
        private const string NotResidingInItaly = "SOGGETTO IDENTIFICATO MA NON RESIDENTE IN ITALIA";

        protected override bool CanMapAddress(string address)
        {
            return !string.IsNullOrWhiteSpace(address) && address != NotResidingInItaly;
        }

        protected override void MapAddress(PersonInformation person, string address)
        {
            var addressParts = address.Split(new[] { "\n" }, StringSplitOptions.None);

            person.Address = addressParts.Length > 0 ? addressParts[0] : null;

            if (addressParts.Length > 1)
            {
                person.City = GetCity(addressParts[1]);
                person.PostalCode = GetPostalCode(addressParts[1]);
                person.Province = GetProvince(addressParts[1]);
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
            if (tokens.Length < 2)
            {
                return null;
            }
            var subset = tokens.Skip(1).Take(tokens.Length - 2).ToArray();
            return string.Join(" ", subset);
        }
    }
}

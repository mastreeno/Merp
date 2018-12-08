using System;
using System.Linq;

namespace Acl.RegistryResolutionServices.Mappers
{
    internal class BEPersonInformationMapper : PersonInformationMapper
    {
        protected override void MapAddress(PersonInformation person, string address)
        {
            var addressParts = address.Split(new[] { "\n" }, StringSplitOptions.None);
            person.Address = addressParts.Length > 0 ? addressParts[0] : null;
            person.City = addressParts.Length > 1 ? GetCity(addressParts[1]) : null;
            person.PostalCode = addressParts.Length > 1 ? GetPostalCode(addressParts[1]) : null;
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

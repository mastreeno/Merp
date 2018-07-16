using System;

namespace Acl.Vies.Mappers
{
    internal class GBPersonInformationMapper : PersonInformationMapper
    {
        protected override void MapAddress(PersonInformation person, string address)
        {
            var addressParts = address.Split(new[] { "\n" }, StringSplitOptions.None);

            switch (addressParts.Length)
            {
                case 5:
                    person.Address = string.Join(" ", addressParts[0], addressParts[1], addressParts[2]).Trim();
                    person.City = string.Join(" ", addressParts[3], addressParts[4]).Trim();
                    break;
                case 6:
                    person.Address = string.Join(" ", addressParts[0], addressParts[1], addressParts[2]).Trim();
                    person.City = addressParts[3];
                    person.Province = addressParts[4];
                    break;
                default:
                    person.Address = address;
                    break;
            }
        }
    }
}

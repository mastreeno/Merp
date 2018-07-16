using System;

namespace Acl.Vies.Mappers
{
    internal class ROPersonInformationMapper : PersonInformationMapper
    {
        protected override void MapAddress(PersonInformation person, string address)
        {
            var addressParts = address.Split(new[] { "\n" }, StringSplitOptions.None);
            person.City = addressParts.Length > 0 ? addressParts[0] : null;
            person.Address = addressParts.Length > 1 ? addressParts[1] : null;
        }
    }
}

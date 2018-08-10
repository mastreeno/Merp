namespace Acl.RegistryResolutionServices.Mappers
{
    internal class DefaultPersonInformationMapper : PersonInformationMapper
    {
        protected override void MapAddress(PersonInformation person, string address)
        {
            person.Address = address;
        }
    }
}

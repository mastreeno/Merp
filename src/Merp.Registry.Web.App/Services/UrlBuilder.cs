namespace Merp.Registry.Web.App.Services
{
    public class UrlBuilder
    {
        public string BuildEditPartyUrl(Guid partyId, string partyType)
        {
            return partyType.ToLower() switch
            {
                "company" => BuildEditCompanyUrl(partyId),
                "person" => BuildEditPersonUrl(partyId),
                _ => throw new ArgumentException("Unknown party type", nameof(partyType))
            };
        }

        public string BuildEditCompanyUrl(Guid partyId)
        {
            return $"/registry/person/edit/{partyId}";
        }

        public string BuildEditPersonUrl(Guid partyId)
        {
            return $"/registry/person/edit/{partyId}";
        }
    }
}

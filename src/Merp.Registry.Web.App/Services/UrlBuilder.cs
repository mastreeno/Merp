namespace Merp.Registry.Web.App.Services
{
    public class UrlBuilder
    {
        public string BuildEditPartyUrl(int partyId, string partyType)
        {
            return partyType.ToLower() switch
            {
                "company" => BuildEditCompanyUrl(partyId),
                "person" => BuildEditPersonUrl(partyId),
                _ => throw new ArgumentException("Unknoiwn party type", nameof(partyType))
            };
        }

        public string BuildEditCompanyUrl(int partyId)
        {
            return $"/registry/person/edit/{partyId}";
        }

        public string BuildEditPersonUrl(int partyId)
        {
            return $"/registry/person/edit/{partyId}";
        }
    }
}

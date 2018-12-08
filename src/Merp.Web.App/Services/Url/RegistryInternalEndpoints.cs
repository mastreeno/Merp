using System;

namespace Merp.Web.App.Services.Url
{
    public class RegistryInternalEndpoints : UrlBuilder.Endpoints
    {
        private readonly string _baseUrl;

        public string GetCountries { get; private set; }

        public string RegisterNewCompany { get; private set; }

        public string GetPartyInfoLocalization { get; private set; }

        public string GetPartyInfoByPattern { get; private set; }

        public string GetPartyInfoById { get; private set; }

        public string LookupPersonByVatNumber { get; private set; }

        public string LookupCompanyByVatNumber { get; private set; }

        public string RegisterNewParty { get; private set; }

        public string GetPersonInfoLocalization { get; private set; }

        public string RegisterNewPerson { get; private set; }

        public string SearchPeopleByPattern { get; private set; }

        public RegistryInternalEndpoints(string registryInternalBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(registryInternalBaseUrl))
            {
                throw new ArgumentException("value cannot be empty", nameof(registryInternalBaseUrl));
            }

            _baseUrl = registryInternalBaseUrl;

            GetCountries = $"{_baseUrl}{ApiPrefix}/Countries/GetCountries";

            #region Company endpoints
            LookupCompanyByVatNumber = $"{_baseUrl}{ApiPrefix}/Company/LookupByVatNumber";
            RegisterNewCompany = $"{_baseUrl}{ApiPrefix}/Company/Register";
            #endregion

            #region Party endpoints
            GetPartyInfoLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Party/PartyInfo";
            GetPartyInfoByPattern = $"{_baseUrl}{ApiPrefix}/Party/GetPartyInfoByPattern";
            GetPartyInfoById = $"{_baseUrl}{ApiPrefix}/Party/GetPartyInfoById";
            RegisterNewParty = $"{_baseUrl}{ApiPrefix}/Party/Register";
            #endregion

            #region Person endpoints
            GetPersonInfoLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Person/PersonInfo";
            LookupPersonByVatNumber = $"{_baseUrl}{ApiPrefix}/Person/LookupByVatNumber";
            RegisterNewPerson = $"{_baseUrl}{ApiPrefix}/Person/Register";
            SearchPeopleByPattern = $"{_baseUrl}{ApiPrefix}/Person/SearchByPattern";
            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.App.Services.Url
{
    public class RegistryEndpoints : UrlBuilder.Endpoints
    {
        private readonly string _baseUrl;

        public string SearchParties { get; private set; }

        public string RegisterNewPerson { get; private set; }

        public string RegisterNewCompany { get; private set; }

        public string UnlistParty { get; private set; }

        public string GetPartyInfoByPattern { get; private set; }

        public string GetPartyInfoById { get; private set; }

        public string PersonInfo { get; private set; }

        public string ChangePersonLegalAddress { get; private set; }

        public string ChangePersonShippingAddress { get; private set; }

        public string ChangePersonBillingAddress { get; private set; }

        public string ChangePersonContactInfo { get; private set; }

        public string SearchPeopleByPattern { get; private set; }

        public string CompanyInfo { get; private set; }

        public string ChangeCompanyName { get; private set; }

        public string ChangeCompanyLegalAddress { get; private set; }

        public string ChangeCompanyShippingAddress { get; private set; }

        public string ChangeCompanyBillingAddress { get; private set; }

        public string ChangeCompanyContactInfo { get; private set; }

        public string AssociateMainContactToCompany { get; private set; }

        public string AssociateAdministrativeContactToCompany { get; private set; }

        public string PersonAddLocalization { get; private set; }

        public string PersonInfoLocalization { get; private set; }

        public string CompanyAddLocalization { get; private set; }

        public string CompanyInfoLocalization { get; private set; }

        public string PartySearchLocalization { get; private set; }

        public RegistryEndpoints(string registryBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(registryBaseUrl))
            {
                throw new ArgumentException("value cannot be empty", nameof(registryBaseUrl));
            }

            _baseUrl = registryBaseUrl;

            #region Party endpoints
            SearchParties = $"{_baseUrl}{ApiPrefix}/Party/Search";
            UnlistParty = $"{_baseUrl}{ApiPrefix}/Party/Unlist";
            PartySearchLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Party/Search";
            GetPartyInfoByPattern = $"{_baseUrl}{ApiPrefix}/Party/GetPartyInfoByPattern";
            GetPartyInfoById = $"{_baseUrl}{ApiPrefix}/Party/GetPartyInfoById";
            #endregion

            #region Person endpoints
            RegisterNewPerson = $"{_baseUrl}{ApiPrefix}/Person/AddEntry";
            PersonInfo = $"{_baseUrl}{ApiPrefix}/Person/GetInfo";
            ChangePersonLegalAddress = $"{_baseUrl}{ApiPrefix}/Person/ChangeLegalAddress";
            ChangePersonShippingAddress = $"{_baseUrl}{ApiPrefix}/Person/ChangeShippingAddress";
            ChangePersonBillingAddress = $"{_baseUrl}{ApiPrefix}/Person/ChangeBillingAddress";
            ChangePersonContactInfo = $"{_baseUrl}{ApiPrefix}/Person/ChangeContactInfo";
            SearchPeopleByPattern = $"{_baseUrl}{ApiPrefix}/Person/SearchByPattern";
            PersonAddLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Person/Add";
            PersonInfoLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Person/Info";
            #endregion

            #region Company endpoints
            RegisterNewCompany = $"{_baseUrl}{ApiPrefix}/Company/AddEntry";
            CompanyInfo = $"{_baseUrl}{ApiPrefix}/Company/GetInfo";
            ChangeCompanyName = $"{_baseUrl}{ApiPrefix}/Company/ChangeCompanyName";
            ChangeCompanyLegalAddress = $"{_baseUrl}{ApiPrefix}/Company/ChangeLegalAddress";
            ChangeCompanyShippingAddress = $"{_baseUrl}{ApiPrefix}/Company/ChangeShippingAddress";
            ChangeCompanyBillingAddress = $"{_baseUrl}{ApiPrefix}/Company/ChangeBillingAddress";
            ChangeCompanyContactInfo = $"{_baseUrl}{ApiPrefix}/Company/ChangeContactInfo";
            AssociateMainContactToCompany = $"{_baseUrl}{ApiPrefix}/Company/AssociateMainContact";
            AssociateAdministrativeContactToCompany = $"{_baseUrl}{ApiPrefix}/Company/AssociateAdministrativeContact";
            CompanyAddLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Company/Add";
            CompanyInfoLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Company/Info";
            #endregion
        }
    }
}

using System;

namespace Merp.Web.App.Services.Url
{
    public class AccountancyInternalEndpoints : UrlBuilder.Endpoints
    {
        private readonly string _baseUrl;

        #region VAT Endpoints
        public string SettingsVatManagementLocalization { get; private set; }

        public string CreateVat { get; private set; }

        public string GetAvailableVats { get; private set; }

        public string GetVatList { get; private set; }

        public string EditVat { get; private set; }

        public string UnlistVat { get; private set; }
        #endregion

        #region Providence funds Endpoints
        public string GetProvidenceFunds { get; private set; }
        #endregion

        #region Withholding taxes Endpoints
        public string GetWithholdingTaxes { get; private set; }
        #endregion

        #region Settings Endpoints
        public string GetSettingsDefaults { get; private set; }

        public string SaveSettingsDefaults { get; private set; }

        public string SettingsDefaultsLocalization { get; private set; }
        #endregion

        public AccountancyInternalEndpoints(string accountancyInternalBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(accountancyInternalBaseUrl))
            {
                throw new ArgumentException("value cannot be empty", nameof(accountancyInternalBaseUrl));
            }

            _baseUrl = accountancyInternalBaseUrl;

            #region VAT Endpoints
            SettingsVatManagementLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Settings/VatManagement";
            CreateVat = $"{_baseUrl}{ApiPrefix}/Vat/Create";
            GetAvailableVats = $"{_baseUrl}{ApiPrefix}/Vat/GetAvailableVats";
            GetVatList = $"{_baseUrl}{ApiPrefix}/Vat/GetList";
            EditVat = $"{_baseUrl}{ApiPrefix}/Vat/Edit";
            UnlistVat = $"{_baseUrl}{ApiPrefix}/Vat/Unlist";
            #endregion

            #region Providence funds Endpoints
            GetProvidenceFunds = $"{_baseUrl}{ApiPrefix}/ProvidenceFund/GetList";
            #endregion

            #region Withholding taxes Endpoints
            GetWithholdingTaxes = $"{_baseUrl}{ApiPrefix}/WithholdingTax/GetList";
            #endregion

            #region Settings Endpoints
            GetSettingsDefaults = $"{_baseUrl}{ApiPrefix}/Settings/GetDefaults";
            SaveSettingsDefaults = $"{_baseUrl}{ApiPrefix}/Settings/SaveDefaults";
            SettingsDefaultsLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Settings/Defaults";
            #endregion
        }
    }
}

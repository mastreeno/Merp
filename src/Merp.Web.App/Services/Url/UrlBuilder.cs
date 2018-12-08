using Merp.Web.App.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Merp.Web.App.Services.Url
{
    public class UrlBuilder
    {
        private readonly EndpointConfiguration _endpoints;

        public AuthorityEndpoints Authority { get; private set; }

        public AuthEndpoints Auth { get; private set; }

        public ClientEndpoints Client { get; private set; }

        public AccountancyEndpoints Accountancy { get; private set; }

        public RegistryEndpoints Registry { get; private set; }

        public RegistryInternalEndpoints RegistryInternal { get; private set; }

        public TimeTrackingEndpoints TimeTracking { get; private set; }

        public UrlBuilder(EndpointConfiguration endpoints)
        {
            _endpoints = endpoints ?? throw new ArgumentNullException(nameof(endpoints));       
            Auth = new AuthEndpoints(_endpoints.Authority);
            Client = new ClientEndpoints(_endpoints.Client);
            Authority = new AuthorityEndpoints(_endpoints.Authority, _endpoints.Client);
            Accountancy = new AccountancyEndpoints(_endpoints.Accountancy);
            Registry = new RegistryEndpoints(_endpoints.Registry);
            RegistryInternal = new RegistryInternalEndpoints(_endpoints.RegistryInternal);
            TimeTracking = new TimeTrackingEndpoints(_endpoints.TimeTracking);
        }

        public virtual string ToJavascriptObject()
        {
            string serializedUrls = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return $"window.endpoints = {serializedUrls}";
        }

        public abstract class Endpoints
        {
            protected string ApiPrefix { get; private set; } = "/api";
            protected string LocalizationApiPrefix { get; private set; } = "/i18n";
        }
    }

    public class AuthorityEndpoints
    {
        public string AuthorityBaseUrl { get; private set; }

        public string ClientCallbackUrl { get; private set; }

        public string ClientBaseUrl { get; private set; }

        public AuthorityEndpoints(string authorityBaseUrl, string clientBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(authorityBaseUrl))
            {
                throw new ArgumentException("value cannot be empty", nameof(authorityBaseUrl));
            }

            if (string.IsNullOrWhiteSpace(clientBaseUrl))
            {
                throw new ArgumentException("value cannot be empty", nameof(clientBaseUrl));
            }

            AuthorityBaseUrl = authorityBaseUrl;
            ClientBaseUrl = clientBaseUrl;
            ClientCallbackUrl = $"{ClientBaseUrl}/static/callback.html";
        }
    }

    public class AuthEndpoints : UrlBuilder.Endpoints
    {
        private readonly string _baseUrl;

        public string ManageProfileLocalization { get; private set; }

        public string ManageProfile { get; private set; }

        public string SendVerificationEmail { get; private set; }

        public AuthEndpoints(string authBaseUrl)
        {
            _baseUrl = authBaseUrl;

            ManageProfileLocalization = $"{_baseUrl}{ApiPrefix}/private{LocalizationApiPrefix}/Manage/Profile";
            ManageProfile = $"{_baseUrl}{ApiPrefix}/private/Manage/Profile";
            SendVerificationEmail = $"{_baseUrl}{ApiPrefix}/private/Manage/SendVerificationEmail";
        }
    }

    public class ClientEndpoints : UrlBuilder.Endpoints
    {
        private readonly string _baseUrl;

        public string GetAppMenuLocalization { get; private set; }

        public string GetHomeIndexLocalization { get; private set; }

        public string GetHomeAboutLocalization { get; private set; }

        public string GetHomeContactLocalization { get; private set; }

        public ClientEndpoints(string clientBaseUrl)
        {
            _baseUrl = clientBaseUrl;

            GetAppMenuLocalization = $"{_baseUrl}{ApiPrefix}/private{LocalizationApiPrefix}/App/Menu";
            GetHomeIndexLocalization = $"{_baseUrl}{ApiPrefix}/private{LocalizationApiPrefix}/Home/Index";
            GetHomeAboutLocalization = $"{_baseUrl}{ApiPrefix}/private{LocalizationApiPrefix}/Home/About";
            GetHomeContactLocalization = $"{_baseUrl}{ApiPrefix}/private{LocalizationApiPrefix}/Home/Contact";
        }
    }
}
